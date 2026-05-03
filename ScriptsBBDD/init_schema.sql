CREATE DATABASE IF NOT EXISTS registro_estudiantes;
CREATE TABLE Estudiantes (
    IdEstudiante INT PRIMARY KEY,
    NombreEstudiante VARCHAR(100) NOT NULL,
    claveAcceso VARCHAR(100) NOT NULL
);

CREATE TABLE Profesores (
    IdProfesor INT AUTO_INCREMENT PRIMARY KEY,
    NombreProfesor VARCHAR(100) NOT NULL
);

CREATE TABLE Materias (
    IdMateria INT AUTO_INCREMENT PRIMARY KEY,
    NombreMateria VARCHAR(100) NOT NULL,
    Creditos INT NOT NULL DEFAULT 3
);

CREATE TABLE ProfesoresMaterias (
    IdProfesor INT,
    IdMateria INT,
    PRIMARY KEY (IdProfesor, IdMateria),
    FOREIGN KEY (IdProfesor) REFERENCES Profesores(IdProfesor),
    FOREIGN KEY (IdMateria) REFERENCES Materias(IdMateria)
);

CREATE TABLE EstudiantesMaterias (
    IdEstudiante INT,
    IdMateria INT,
    PRIMARY KEY (IdEstudiante, IdMateria),
    FOREIGN KEY (IdEstudiante) REFERENCES Estudiantes(IdEstudiante),
    FOREIGN KEY (IdMateria) REFERENCES Materias(IdMateria)
);

-- Insertar estudiantes
INSERT INTO Profesores (NombreProfesor) VALUES 
('Prof. Juan'), 
('Prof. María'), 
('Prof. Pedro'), 
('Prof. Ana'), 
('Prof. Luis');

-- Insertar materias
INSERT INTO Materias (NombreMateria) VALUES 
('Matemáticas'),     -- 1
('Física'),          -- 2
('Química'),         -- 3
('Historia'),        -- 4
('Literatura'),      -- 5
('Biología'),        -- 6
('Geografía'),       -- 7
('Arte'),            -- 8
('Computación'),     -- 9
('Inglés');          -- 10

-- Asignar materias a profesores (2 materias por profe)
INSERT INTO ProfesoresMaterias (IdProfesor, IdMateria) VALUES 
(1, 1), (1, 2),       -- Prof. Juan
(2, 3), (2, 4),       -- Prof. María
(3, 5), (3, 6),       -- Prof. Pedro
(4, 7), (4, 8),       -- Prof. Ana
(5, 9), (5, 10);      -- Prof. Luis

-- SP para registrar materias de un estudiante
DELIMITER $$

DROP PROCEDURE IF EXISTS sp_RegistrarMateriasEstudiante$$

CREATE PROCEDURE sp_RegistrarMateriasEstudiante (
    IN p_IdEstudiante INT,
    IN p_MateriasJSON JSON
)
BEGIN
    DECLARE materia_id INT;
    DECLARE prof_id INT;
    DECLARE total_materias INT;
    DECLARE i INT DEFAULT 0;
    DECLARE materias_actuales INT;
    DECLARE profesores_distintos INT;

    -- Validar cantidad de materias a inscribir
    SET total_materias = JSON_LENGTH(p_MateriasJSON);

    -- Validar si ya tiene materias inscritas
    SELECT COUNT(*) INTO materias_actuales FROM EstudiantesMaterias WHERE IdEstudiante = p_IdEstudiante;
    IF (materias_actuales + total_materias) > 3 THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Puede inscribir un máximo de 3 materias.';
    END IF;

    IF total_materias > 3 THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'El estudiante debe registrar máximo 3 materias.';
    END IF;

    -- Crear tabla temporal para almacenar materias y profesores
    CREATE TEMPORARY TABLE IF NOT EXISTS TempMaterias (
        IdMateria INT,
        IdProfesor INT
    );

    -- Llenar tabla temporal desde el JSON
    WHILE i < total_materias DO
        SET materia_id = CAST(JSON_UNQUOTE(JSON_EXTRACT(p_MateriasJSON, CONCAT('$[', i, ']'))) AS UNSIGNED);

        -- Validar que exista un profesor asignado a la materia
        SELECT IdProfesor INTO prof_id FROM ProfesoresMaterias WHERE IdMateria = materia_id LIMIT 1;

        IF prof_id IS NULL THEN
            SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Una materia no tiene profesor asignado.';
        END IF;

        INSERT INTO TempMaterias (IdMateria, IdProfesor) VALUES (materia_id, prof_id);

        SET i = i + 1;
    END WHILE;

    -- Validar que no se repitan profesores
    SELECT COUNT(DISTINCT IdProfesor) INTO profesores_distintos FROM TempMaterias;
    IF profesores_distintos < total_materias THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Las materias seleccionadas comparten profesor. No permitido.';
    END IF;

    -- Insertar materias
    INSERT INTO EstudiantesMaterias (IdEstudiante, IdMateria)
    SELECT p_IdEstudiante, IdMateria FROM TempMaterias;

    -- Eliminar tabla temporal
    DROP TEMPORARY TABLE IF EXISTS TempMaterias;
END$$

DELIMITER ;

-- SP Obtiene las materias disponibles para un estudiante
DELIMITER $$

DROP PROCEDURE IF EXISTS sp_ObtenerMateriasDisponibles$$

CREATE PROCEDURE sp_ObtenerMateriasDisponibles (
    IN p_IdEstudiante INT
)
BEGIN
    SELECT m.IdMateria, m.NombreMateria, m.Creditos, p.IdProfesor, p.NombreProfesor
    FROM Materias m
    INNER JOIN ProfesoresMaterias mp ON m.IdMateria = mp.IdMateria
    INNER JOIN Profesores p ON mp.IdProfesor = p.IdProfesor
    WHERE m.IdMateria NOT IN (
        SELECT IdMateria FROM EstudiantesMaterias WHERE IdEstudiante = p_IdEstudiante
    );
END$$

DELIMITER ;

--SP Retorna los nombres de estudiantes que están en las mismas materias que el estudiante actual (sin incluirlo):
DELIMITER $$

DROP PROCEDURE IF EXISTS sp_VerEstudiantesPorMateria$$

CREATE PROCEDURE sp_VerEstudiantesPorMateria (
    IN p_IdEstudiante INT
)
BEGIN
    SELECT DISTINCT e2.NombreEstudiante, em.IdMateria, m.NombreMateria
    FROM EstudiantesMaterias em
    JOIN EstudiantesMaterias em2 ON em.IdMateria = em2.IdMateria
    JOIN Estudiantes e2 ON em2.IdEstudiante = e2.IdEstudiante
    INNER JOIN Materias m ON m.IdMateria = em.IdMateria
    WHERE em.IdEstudiante = p_IdEstudiante
      AND em2.IdEstudiante != p_IdEstudiante
    ORDER BY em.IdMateria;
END $$
DELIMITER;


-- SP Registro de estudiantes

DELIMITER $$

DROP PROCEDURE IF EXISTS sp_RegistrarEstudiante$$

CREATE PROCEDURE sp_RegistrarEstudiante(
    IN p_IdEstudiante INT,
    IN p_Nombre VARCHAR(100),
    IN p_ClaveAcceso VARCHAR(255)
)
BEGIN
    INSERT INTO Estudiantes (IdEstudiante, NombreEstudiante, claveAcceso)
    VALUES (p_IdEstudiante, p_Nombre, p_ClaveAcceso);
END $$
DELIMITER ;

-- SP Login de estudiantes
DELIMITER $$

DROP PROCEDURE IF EXISTS sp_LoginEstudiante$$

CREATE PROCEDURE sp_LoginEstudiante(
    IN p_IdEstudiante INT
)
BEGIN
    SELECT IdEstudiante, NombreEstudiante, claveAcceso
    FROM Estudiantes
    WHERE IdEstudiante = p_IdEstudiante;
END $$

DELIMITER ;

-- Sp para obtener las materias registradas por un estudiante
DELIMITER $$
DROP PROCEDURE IF EXISTS sp_ObtenerMateriasRegistradas$$
CREATE PROCEDURE sp_ObtenerMateriasRegistradas(
    IN p_IdEstudiante INT
)
BEGIN
    SELECT m.IdMateria, m.NombreMateria, m.Creditos
    FROM EstudiantesMaterias em
    INNER JOIN Materias m ON m.IdMateria = em.IdMateria
    WHERE em.IdEstudiante = p_IdEstudiante;
END $$
DELIMITER ;