# RiwiLens Backend

# Descripción General

RiwiLens Backend es un sistema robusto de gestión académica diseñado específicamente para el ecosistema educativo de Riwi. Proporciona una API RESTful completa para administrar estudiantes (coders), líderes de equipo (team leaders), organización en clanes, seguimiento de asistencias, evaluación de habilidades y retroalimentación estructurada.


# Características Principales
Gestión de Usuarios

👨‍💻 Coders: Perfiles completos de estudiantes con información académica
👔 Team Leaders: Gestión de líderes con asignación de clanes
🔐 Autenticación y Autorización: Sistema seguro basado en roles

Organización Académica

🏆 Clanes: Agrupación de coders con líderes asignados
📅 Clases: Programación y gestión de sesiones educativas
⏰ Horarios: Gestión temporal de actividades

Seguimiento y Evaluación

✔️ Asistencias: Registro detallado de presencia y participación
💼 Habilidades Técnicas: Evaluación de competencias técnicas
🤝 Habilidades Blandas: Medición de soft skills
📝 Feedback: Sistema estructurado de retroalimentación

Sistema de Notificaciones

🔔 Alertas en tiempo real: Notificaciones instantáneas
📧 Comunicación efectiva: Mensajería dirigida y contextual

# Arquitectura

RiwiLens Backend implementa Clean Architecture con principios de Domain-Driven Design (DDD), garantizando:

Independencia de frameworks: El dominio no depende de tecnologías específicas
Testabilidad: Cada capa puede probarse de forma aislada
Mantenibilidad: Separación clara entre lógica de negocio e infraestructura
Escalabilidad: Fácil adaptación a nuevos requisitos

# Flujo de Arquitectura

┌─────────────────────────────────────────────┐
│         RiwiLens.Api (API Layer)            │
│      Controllers • Middleware • DTOs        │
└─────────────────────────────────────────────┘
                    ↓ ↑
┌─────────────────────────────────────────────┐
│    RiwiLens.Application (Use Cases)         │
│     Services • Interfaces • Commands        │
└─────────────────────────────────────────────┘
                    ↓ ↑
┌─────────────────────────────────────────────┐
│      RiwiLens.Domain (Core Business)        │
│    Entities • Value Objects • Enums         │
└─────────────────────────────────────────────┘
                    ↓ ↑
┌─────────────────────────────────────────────┐
│  RiwiLens.Infrastructure (Data & External)  │
│   EF Core • Repositories • Configurations   │
└─────────────────────────────────────────────┘

# Principios Aplicados

✅ SOLID Principles: Cada componente tiene responsabilidad única y bien definida
✅ Dependency Inversion: Las capas superiores no dependen de las inferiores
✅ Clean Code: Código legible, mantenible y expresivo
✅ Repository Pattern: Abstracción completa del acceso a datos
✅ Unit of Work: Gestión transaccional coherente


# Stack Tecnológico
# Backend Core

Framework: .NET 8.0 LTS
Lenguaje: C# 
Runtime: ASP.NET Core 8.0

# Persistencia

ORM: Entity Framework Core 8.0
Base de Datos: PostgreSQL 15+
Migraciones: EF Core Migrations

# Seguridad

Autenticación: JWT Bearer Tokens
Autorización: Role-Based Access Control (RBAC)
Identity: ASP.NET Core Identity

# Documentación

API Docs: Swagger/OpenAPI 3.0
Especificación: Swashbuckle.AspNetCore

Herramientas de Desarrollo

IDE Recomendado: JetBrains Rider / Visual Studio 2022
Control de Versiones: Git
Package Manager: NuGet

# Estructura del Proyecto

RiwiLens-Backend/
│
├── src/
│   │
│   ├── RiwiLens.Api/                      # 🌐 Capa de Presentación
│   │   ├── Controllers/                   # Endpoints REST
│   │   ├── Middleware/                    # Interceptores HTTP
│   │   ├── Extensions/                    # Métodos de extensión
│   │   ├── Properties/                    # Configuración de launch
│   │   ├── Program.cs                     # Punto de entrada
│   │   └── appsettings.json              # Configuración
│   │
│   ├── RiwiLens.Application/             # 📋 Capa de Aplicación
│   │   ├── Services/                      # Lógica de negocio
│   │   ├── Interfaces/                    # Contratos de servicio
│   │   ├── DTOs/                         # Data Transfer Objects
│   │   ├── Commands/                      # Operaciones CQRS
│   │   ├── Queries/                       # Consultas CQRS
│   │   └── Validators/                    # Validaciones FluentValidation
│   │
│   ├── RiwiLens.Domain/                  # 💎 Capa de Dominio
│   │   ├── Entities/                      # Entidades de negocio
│   │   │   ├── Coder.cs
│   │   │   ├── TeamLeader.cs
│   │   │   ├── Clan.cs
│   │   │   ├── Attendance.cs
│   │   │   ├── Feedback.cs
│   │   │   ├── Class.cs
│   │   │   ├── TechnicalSkill.cs
│   │   │   ├── SoftSkill.cs
│   │   │   └── Notification.cs
│   │   ├── Enums/                         # Enumeraciones
│   │   ├── ValueObjects/                  # Objetos de valor DDD
│   │   └── Exceptions/                    # Excepciones de dominio
│   │
│   └── RiwiLens.Infrastructure/          # 🔧 Capa de Infraestructura
│       ├── Persistence/                   # Contexto EF Core
│       │   ├── ApplicationDbContext.cs
│       │   └── Repositories/              # Implementación repositorios
│       ├── Configurations/                # Configuraciones EF
│       ├── Identity/                      # Configuración Identity
│       └── Migrations/                    # Migraciones de BD
│
├── tests/
│   └── RiwiLens.Tests/                   # 🧪 Suite de Pruebas
│       ├── Unit/                          # Pruebas unitarias
│       ├── Integration/                   # Pruebas de integración
│       └── Fixtures/                      # Datos de prueba
│
├── docs/                                  # 📚 Documentación
│   ├── architecture.md
│   ├── api-endpoints.md
│   └── database-schema.md
│
├── .gitignore
├── .env.example                           # Plantilla de variables
├── RiwiLens.sln                          # Solución .NET
└── README.md                             # Este archivo



# Requisitos Previos

Asegúrate de tener instalado lo siguiente:
HerramientaVersión MínimaEnlace de Descarga.NET SDK8.0+DownloadPostgreSQL15.0+DownloadGit2.30+DownloadIDERider/VS 2022Rider • VS

Verificar Instalación

# Verificar .NET
dotnet --version

# Verificar PostgreSQL
psql --version

# Verificar Git
git --version



 # Instalación
1. Clonar el Repositorio

# HTTPS
git clone https://github.com/SebastianRdev/RiwiLens-Backend.git

# SSH (recomendado si tienes llaves configuradas)
git clone git@github.com:SebastianRdev/RiwiLens-Backend.git

# Navegar al directorio
cd RiwiLens-Backend


2. Restaurar Dependencias

# Restaurar todos los paquetes NuGet
dotnet restore

# O restaurar la solución completa
dotnet restore RiwiLens.sln


3. Verificar Compilación

# Compilar el proyecto
dotnet build --configuration Debug

# Compilar para producción
dotnet build --configuration Release


 # Configuración
1. Variables de Entorno
Crea un archivo .env en la raíz del proyecto basándote en .env.example:

# === DATABASE CONFIGURATION ===
DB_HOST=localhost
DB_PORT=5432
DB_NAME=riwilens_db
DB_USER=postgres
DB_PASSWORD=tu_password_seguro

# Cadena de conexión completa
DB_CONNECTION_STRING=Host=${DB_HOST};Port=${DB_PORT};Database=${DB_NAME};Username=${DB_USER};Password=${DB_PASSWORD};

# === API CONFIGURATION ===
API_PORT=5001
ASPNETCORE_ENVIRONMENT=Development
ASPNETCORE_URLS=https://localhost:5001;http://localhost:5000

# === JWT CONFIGURATION ===
JWT_SECRET=TU_SUPER_SECRET_KEY_MINIMO_32_CARACTERES_AQUI
JWT_ISSUER=RiwiLens.Api
JWT_AUDIENCE=RiwiLens.Client
JWT_EXPIRATION_MINUTES=60

# === SWAGGER CONFIGURATION ===
SWAGGER_ENABLED=true
SWAGGER_ROUTE_PREFIX=swagger

# === LOGGING ===
LOG_LEVEL=Information


2. appsettings.json
Actualiza src/RiwiLens.Api/appsettings.json:

{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=riwilens_db;Username=postgres;Password=your_password"
  },
  "Jwt": {
    "Secret": "tu_secret_key_minimo_32_caracteres_largo",
    "Issuer": "RiwiLens.Api",
    "Audience": "RiwiLens.Client",
    "ExpirationMinutes": 60
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore": "Information"
    }
  },
  "AllowedHosts": "*"
}


3. Crear Base de Datos

# Conectar a PostgreSQL
psql -U postgres

# Crear base de datos
CREATE DATABASE riwilens_db;

# Verificar
\l

# Salir
\q


4. Aplicar Migraciones

# Navegar a la capa de API
cd src/RiwiLens.Api

# Aplicar migraciones
dotnet ef database update --project ../RiwiLens.Infrastructure

# Verificar migraciones aplicadas
dotnet ef migrations list --project ../RiwiLens.Infrastructure


# Ejecución

# Desarrollo Local

# Desde la raíz del proyecto
cd src/RiwiLens.Api

# Ejecutar en modo desarrollo
dotnet run

# O con hot reload
dotnet watch run


# Acceder a la Aplicación
# Una vez iniciado, la API estará disponible en:

ServicioURLDescripciónAPI HTTPShttps://localhost:5001Endpoint seguro principalAPI HTTPhttp://localhost:5000Endpoint HTTP (desarrollo)Swagger UIhttps://localhost:5001/swaggerDocumentación interactivaHealth Checkhttps://localhost:5001/healthEstado del sistema


 # Modelo de Datos
# Diagrama Entidad-Relación

┌─────────────┐         ┌──────────────┐         ┌─────────────┐
│    Coder    │────┬────│     Clan     │────┬────│ TeamLeader  │
└─────────────┘    │    └──────────────┘    │    └─────────────┘
       │           │            │            │
       │           │            │            │
       ├───────────┘            └────────────┘
       │
       ├────────────┐
       │            │
┌──────▼──────┐ ┌──▼──────────┐
│ Attendance  │ │  Feedback   │
└─────────────┘ └─────────────┘
       │
       │
┌──────▼──────┐
│    Class    │
└─────────────┘
       │
┌──────▼──────────────┐
│  TechnicalSkill     │
│  SoftSkill          │
│  Notification       │
└─────────────────────┘


# Entidades Principales
👨‍💻 Coder (Estudiante)

public class Coder
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime EnrollmentDate { get; set; }
    public Guid? ClanId { get; set; }
    public Clan Clan { get; set; }
    public ICollection<Attendance> Attendances { get; set; }
    public ICollection<Feedback> Feedbacks { get; set; }
}


 # TeamLeader (Líder de Equipo)

 public class TeamLeader
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Specialty { get; set; }
    public ICollection<Clan> Clans { get; set; }
}

 # Clan (Equipo)

 public class Clan
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid TeamLeaderId { get; set; }
    public TeamLeader TeamLeader { get; set; }
    public ICollection<Coder> Coders { get; set; }
}

# Attendance (Asistencia)

public class Attendance
{
    public Guid Id { get; set; }
    public Guid CoderId { get; set; }
    public Guid ClassId { get; set; }
    public DateTime Date { get; set; }
    public AttendanceStatus Status { get; set; }
    public Coder Coder { get; set; }
    public Class Class { get; set; }
}



# Enumeraciones

public enum AttendanceStatus
{
    Present,
    Absent,
    Late,
    Excused
}

public enum FeedbackType
{
    Technical,
    SoftSkill,
    General
}

public enum SkillLevel
{
    Beginner,
    Intermediate,
    Advanced,
    Expert
}


# Endpoints de API

# Autenticación
Método Endpoint Descripción

POST/api/auth/loginIniciar sesión
POST/api/auth/registerRegistrar usuario
POST/api/auth/refresh
Renovar token 
POST/api/auth/logout Cerrar sesión


# Coders

Método Endpoint Descripción

GET/api/codersListar todos los coders
GET/api/coders/{id}Obtener coder por ID
POST/api/codersCrear nuevo coder
PUT/api/coders/{id}Actualizar coder
DELETE/api/coders/{id}Eliminar coder
GET/api/coders/{id}/attendancesAsistencias del coder
GET/api/coders/{id}/feedbacksFeedback del coder


# Team Leaders

Método Endpoint Descripción

GET/api/teamleadersListar team leaders
GET/api/teamleaders/{id}Obtener team leader
POST/api/teamleadersCrear team leader
PUT/api/teamleaders/{id}Actualizar team leader
DELETE/api/teamleaders/{id}Eliminar team leader

# Clanes

Método Endpoint Descripción

GET/api/clansListar clanes
GET/api/clans/{id}Obtener clan por ID
POST/api/clansCrear clan
PUT/api/clans/{id}Actualizar clan
DELETE/api/clans/{id}Eliminar clan
GET/api/clans/{id}/codersCoders del clan


# Asistencias

Método Endpoint Descripción

GET/api/attendancesListar asistencias
GET/api/attendances/{id}Obtener asistencia
POST/api/attendancesRegistrar asistencia
PUT/api/attendances/{id}Actualizar asistencia
DELETE/api/attendances/{id}Eliminar asistencia

# Feedback

Método Endpoin tDescripción

GET/api/feedbacksListar feedbacks
GET/api/feedbacks/{id}Obtener feedback
POST/api/feedbacksCrear feedback
PUT/api/feedbacks/{id}Actualizar feedback
DELETE/api/feedbacks/{id}Eliminar feedback

 # Documentación Completa

 https://localhost:5001/swagger


# Testing

Estructura de Pruebas

RiwiLens.Tests/
├── Unit/                    # Pruebas unitarias
│   ├── Domain/
│   ├── Application/
│   └── Infrastructure/
├── Integration/             # Pruebas de integración
│   ├── Controllers/
│   └── Repositories/
└── Fixtures/               # Datos de prueba
    └── TestData.cs


# Ejecutar Pruebas

# Todas las pruebas
dotnet test

# Con cobertura de código
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover

# Solo pruebas unitarias
dotnet test --filter "Category=Unit"

# Solo pruebas de integración
dotnet test --filter "Category=Integration"

# Con logs detallados
dotnet test --logger "console;verbosity=detailed"


# Ejemplo de Prueba Unitaria

[Fact]
public async Task CreateCoder_WithValidData_ShouldReturnCoder()
{
    // Arrange
    var coder = new Coder
    {
        FirstName = "John",
        LastName = "Doe",
        Email = "john.doe@example.com"
    };

    // Act
    var result = await _coderService.CreateAsync(coder);

    // Assert
    result.Should().NotBeNull();
    result.Email.Should().Be("john.doe@example.com");
}


# Deployment

# Compilación para Producción 

# Compilar en modo Release
dotnet build --configuration Release

# Publicar aplicación
dotnet publish --configuration Release --output ./publish

# Verificar artefactos
ls -la ./publish



# Configuración para Producción

# appsettings.Production.json

{
  "ConnectionStrings": {
    "DefaultConnection": "${DB_CONNECTION_STRING}"
  },
  "Jwt": {
    "Secret": "${JWT_SECRET}",
    "Issuer": "${JWT_ISSUER}",
    "Audience": "${JWT_AUDIENCE}",
    "ExpirationMinutes": 30
  },
  "Logging": {
    "LogLevel": {
      "Default": "Warning",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore": "Error"
    }
  },
  "AllowedHosts": "*.riwilens.com"
}


# Comandos Útiles
# Entity Framework

# Crear nueva migración
dotnet ef migrations add NombreMigracion --project src/RiwiLens.Infrastructure --startup-project src/RiwiLens.Api

# Aplicar migraciones
dotnet ef database update --project src/RiwiLens.Infrastructure --startup-project src/RiwiLens.Api

# Listar migraciones
dotnet ef migrations list --project src/RiwiLens.Infrastructure

# Revertir última migración
dotnet ef migrations remove --project src/RiwiLens.Infrastructure --startup-project src/RiwiLens.Api

# Generar script SQL
dotnet ef migrations script --project src/RiwiLens.Infrastructure --startup-project src/RiwiLens.Api --output migration.sql

# Eliminar base de datos
dotnet ef database drop --project src/RiwiLens.Infrastructure --startup-project src/RiwiLens.Api


# Formateo y Análisis

# Formatear código
dotnet format

# Análisis de código
dotnet build /p:TreatWarningsAsErrors=true

# Verificar estilo
dotnet format --verify-no-changes


# Coders:


# Equipo RiwiLens
Daniela Martinez
Sebastian Reyes 
Andres Camilo Toloza 
Camilo Andres Rodriguez
Jesus Castro 
Juan David Guzman...










