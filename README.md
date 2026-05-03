# Registro Estudiantes API

API REST desarrollada en **.NET 8** para gestionar autenticación de estudiantes, consulta de materias, registro de materias y visualización de compañeros.

El proyecto está construido aplicando **Arquitectura Hexagonal**, separando la lógica de aplicación de detalles externos como HTTP, MySQL, Dapper, JWT y BCrypt.

---

## Arquitectura

La solución usa el enfoque de **puertos y adaptadores**:

```txt
Cliente HTTP
   ↓
API / Controllers
   ↓
Puertos de entrada
   ↓
Casos de uso
   ↓
Puertos de salida
   ↓
Infraestructura
   ↓
MySQL
```

### Capas

| Proyecto | Responsabilidad |
|---|---|
| `RegistroEstudiantes.API` | Adaptador de entrada HTTP. Controllers, Swagger, JWT, CORS y middlewares. |
| `RegistroEstudiantes.Aplicacion` | Casos de uso y puertos de entrada/salida. |
| `RegistroEstudiante.Domain` | Entidades del dominio. |
| `RegistroEstudiantes.Infraestructura` | Adaptadores de salida: Dapper, MySQL, BCrypt y JWT. |
| `ScriptsBBDD` | Scripts y configuración local de base de datos. |

---

## Tecnologías

- .NET 8
- ASP.NET Core Web API
- MySQL
- Dapper
- JWT Bearer
- BCrypt
- Swagger / OpenAPI
- Docker Compose

---

## Estructura general

```txt
RegistroEstudiantes_Hex/
│
├── RegistroEstudiantes.API/
├── RegistroEstudiantes.Aplicacion/
├── RegistroEstudiante.Domain/
├── RegistroEstudiantes.Infraestructura/
├── ScriptsBBDD/
├── RegistroEstudiantesAPI.sln
└── README.md
```

---

## Configuración

La configuración principal está en:

```txt
RegistroEstudiantes.API/appsettings.json
```

Ejemplo:

```json
{
  "ConnectionStrings": {
    "ConexionBaseDeDatos": "server=localhost;port=3307;user=TU_USUARIO;password=TU_PASSWORD;database=registro_estudiantes"
  },
  "Jwt": {
    "ClaveSecreta": "REEMPLAZAR_POR_UNA_CLAVE_SEGURA_DE_AL_MENOS_32_CARACTERES",
    "ExpiracionHoras": 1
  },
  "AllowedHosts": "*"
}
```

> Para ambientes reales se recomienda usar variables de entorno, User Secrets o un administrador de secretos.

---

## Base de datos

El proyecto usa **MySQL** y procedimientos almacenados mediante **Dapper**.

Procedimientos principales:

```txt
sp_RegistrarEstudiante
sp_LoginEstudiante
sp_RegistrarMateriasEstudiante
sp_ObtenerMateriasDisponibles
sp_VerEstudiantesPorMateria
sp_ObtenerMateriasRegistradas
```

Para levantar la base de datos local con Docker:

```bash
cd ScriptsBBDD
docker compose up -d
```

---

## Ejecución local

Desde la raíz del proyecto:

```bash
dotnet restore
dotnet build
dotnet run --project RegistroEstudiantes.API
```

Luego abrir Swagger en la URL mostrada por consola, por ejemplo:

```txt
https://localhost:5001/swagger
```

---

## Autenticación

La API utiliza **JWT Bearer Token**.

Flujo básico:

1. Registrar estudiante.
2. Iniciar sesión.
3. Copiar el token recibido.
4. Usar el token en Swagger/Postman con el formato:

```txt
Bearer {token}
```

---

## Endpoints principales

### Autenticación

| Método | Endpoint | Requiere token | Descripción |
|---|---|---|---|
| POST | `/api/autenticacion/registro` | No | Registra un estudiante. |
| POST | `/api/autenticacion/login` | No | Autentica y retorna JWT. |

### Estudiantes

| Método | Endpoint | Requiere token | Descripción |
|---|---|---|---|
| GET | `/api/estudiantes/materias-disponibles` | Sí | Consulta materias disponibles. |
| POST | `/api/estudiantes/registrar-materias` | Sí | Registra materias del estudiante. |
| GET | `/api/estudiantes/companeros` | Sí | Consulta compañeros por materias. |
| GET | `/api/estudiantes/materias-inscritas` | Sí | Consulta materias inscritas. |

Ejemplo para registrar materias:

```json
{
  "materias": [1, 2, 3]
}
```

---

## Decisiones técnicas

- Se usa **Arquitectura Hexagonal** para desacoplar la aplicación de frameworks y detalles técnicos.
- Los **controllers** actúan como adaptadores de entrada.
- Los **casos de uso** contienen la lógica de aplicación.
- Los **puertos de salida** definen contratos para persistencia, hashing y generación de tokens.
- La **infraestructura** implementa esos contratos usando Dapper, MySQL, BCrypt y JWT.
- Se usa **Dapper** por su simplicidad y eficiencia con procedimientos almacenados.
- Las contraseñas se almacenan usando **hash BCrypt**, no texto plano.

---

## Seguridad

El proyecto incluye:

- Autenticación JWT.
- Endpoints protegidos con `[Authorize]`.
- Hashing de contraseñas con BCrypt.
- Middleware global para manejo de errores.
- Configuración externa mediante `appsettings.json`.

Recomendaciones para producción:

- No publicar secretos reales.
- Usar HTTPS.
- Restringir CORS.
- Usar variables de entorno para credenciales.
- Configurar expiración adecuada del token.

---
