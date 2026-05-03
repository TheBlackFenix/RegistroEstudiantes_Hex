# RegistroEstudiantes API

Resumen de la solución para el proyecto de Registro de Estudiantes.

## Descripción

Aplicación backend construida con .NET 8 que expone una API para el registro de materias de estudiantes, administración de compañeros y consulta de materias disponibles. La solución está organizada por capas (API, Aplicación, Infraestructura y Dominio) siguiendo principios de arquitectura limpia.

## Estructura del repositorio

- RegistroEstudiantes.API/       -> Proyecto Web API (controladores, DTOs, middlewares, configuraciones)
- RegistroEstudiantes.Aplicacion/-> Lógica de negocio y casos de uso (puertos de entrada)
- RegistroEstudiante.Dominio/    -> Entidades del dominio
- RegistroEstudiantes.Infraestructura/ -> Implementaciones de repositorios, adaptadores y servicios (persistencia, seguridad, tokens)

## Requisitos

- .NET 8 SDK
- MySQL (o servidor compatible) con las tablas y procedimientos almacenados necesarios
- (Opcional) Postman o similar para probar endpoints

## Configuración

1. Clonar el repositorio.
2. Editar el archivo appsettings.json en RegistroEstudiantes.API para configurar la cadena de conexión y ajustes de JWT. Valores importantes:
   - ConnectionStrings:ConexionBaseDeDatos: cadena de conexión a la base de datos MySQL.
   - Sección de JWT (revisar el contenido de `appsettings.json` y las extensiones de autenticación para las claves exactas).

3. Verificar que la base de datos contiene las tablas y procedimientos almacenados utilizados por el repositorio. Los procedimientos que usa la aplicación (ejemplos) pueden incluir:
   - sp_RegistrarMateriasEstudiante
   - sp_ObtenerMateriasDisponibles
   - sp_VerEstudiantesPorMateria
   - sp_ObtenerMateriasRegistradas

Si no dispone de los scripts SQL, consulte la carpeta `ScriptsBBDD/`.

## Cómo ejecutar

Desde la raíz del repositorio:

1. Restaurar paquetes:
   dotnet restore

2. Construir el proyecto:
   dotnet build

3. Ejecutar la API (desde el proyecto RegistroEstudiantes.API):
   dotnet run --project RegistroEstudiantes.API

La API por defecto se ejecutará en la URL indicada por la configuración (ej. https://localhost:5001).

## Autenticación

La API utiliza autenticación JWT. Antes de usar los endpoints protegidos de `EstudiantesController` debe autenticarse (revisar el `AutenticacionController` para endpoints de login/registro). Incluir el token en el header `Authorization: Bearer <token>`.

## Endpoints principales

Nota: el prefijo de ruta base es `api/`.

- GET api/estudiantes/materias-disponibles
  - Obtiene las materias disponibles para el estudiante autenticado.

- POST api/estudiantes/registrar-materias
  - Registra materias para el estudiante autenticado.
  - Request body: { "materias": [1, 2, 3] }

- GET api/estudiantes/companeros
  - Obtiene compañeros por materias del estudiante autenticado.

- GET api/estudiantes/materias-inscritas
  - Obtiene las materias inscritas por el estudiante autenticado.

- Auth endpoints
  - Revisar `RegistroEstudiantes.API.Controllers.AutenticacionController` para los endpoints de autenticación (login / registro).

## Persistencia

La capa de infraestructura usa Dapper para ejecutar procedimientos almacenados en MySQL. Asegúrese de que la cadena de conexión en `appsettings.json` apunte a una base de datos con los procedimientos esperados.
