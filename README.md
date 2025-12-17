# RiwiLens Backend

## Overview
RiwiLens Backend is a production‑ready RESTful API that manages the academic ecosystem of Riwi. It handles **Coders**, **Team Leaders**, **Clans**, **Classes**, **Attendances**, **Technical & Soft Skills**, and **Feedback**. The API is secured with **JWT Bearer Tokens** stored in HttpOnly cookies and uses role‑based access control (Coder, TeamLeader, Admin).

## Deployment
- **Swagger UI** (interactive API docs): http://riwilensapisingle.eba-dyfsksjm.us-east-1.elasticbeanstalk.com/swagger/index.html
- **Database**: AWS RDS PostgreSQL (managed, no local DB setup required).

## Authentication
| Endpoint | Method | Description |
|---|---|---|
| `/api/auth/login` | POST | Authenticates user credentials, issues a JWT and stores it in an HttpOnly cookie. |
| `/api/auth/logout` | POST | Clears authentication cookies, effectively logging the user out. |
| `/api/auth/me` | GET | Returns the full profile of the authenticated user: `id`, `uuid`, `email`, `name`, `role`, `documentType`, `documentNumber`, `gender`, `isActive`, `profileImageUrl`, etc. |

All protected endpoints require the `[Authorize]` attribute and enforce role policies where appropriate.

## API Endpoints
### Auth
- `POST /api/auth/login`
- `POST /api/auth/logout`
- `GET /api/auth/me`

### Coders
- `GET /api/coders`
- `GET /api/coders/{id}`
- `POST /api/coders`
- `PUT /api/coders/{id}`
- `GET /api/coders/{id}/attendances`

### Clans
- `GET /api/Clans`
- `GET /api/Clans/{id}`
- `POST /api/Clans`
- `PUT /api/Clans/{id}`
- `DELETE /api/Clans/{id}`
- `POST /api/Clans/{id}/coders`
- `DELETE /api/Clans/{id}/coders/{coderId}`
- `POST /api/Clans/{id}/team-leaders`
- `DELETE /api/Clans/{id}/team-leaders/{tlId}`

### Catalog
- `GET /api/Catalog/technical-skills`
- `GET /api/Catalog/soft-skills`
- `GET /api/Catalog/class-types`
- `GET /api/Catalog/days`
- `GET /api/Catalog/specialties`
- `GET /api/Catalog/status-coders`

### Users
- `GET /api/Users`
- `GET /api/Users/{id}`
- `POST /api/Users`
- `PUT /api/Users/{id}`
- `DELETE /api/Users/{id}`

### Attendances
- `GET /api/Attendance`
- `GET /api/Attendance/class/{classId}`
- `GET /api/Attendance/coder/{coderId}`
- `GET /api/Attendance/clan/{clanId}/date/{date}`
- `PUT /api/Attendance/{id}`

### Cv
- `GET /api/Cv/generate/{coderId}`

### Dashboard
- `GET /api/Dashboard/stats`
- `GET /api/Dashboard/user-management-stats`
- `GET /api/Dashboard/users`
- `GET /api/Dashboard/coder/{coderId}`
- `GET /api/Dashboard/teamleader/{teamleaderId}`

### Report
- `GET /api/Report/attendances`
- `GET /api/Report/feedback`

### Feedback
- `GET /api/Feedback/coder/{coderId}`
- `GET /api/Feedback/teamleader/{teamleaderId}`
- `POST /api/Feedback`

### Notification
- `GET /api/Notification`
- `POST /api/Notification`
- `PUT /api/Notification/{id}/read`


## Domain Entities (Properties)
### Attendance
- `Id: int` (PK)
- `ClanId: int` (FK → Clan)
- `ClassId: int` (FK → Class)
- `CoderId: int` (FK → Coder)
- `TimestampIn: DateTime`
- `Status: AttendanceStatus` (enum)
- `VerifiedBy: string`
- `ImageUrl: string`
- Navigation properties: `Clan`, `Class`, `Coder`

### Coder
- `Id: int` (PK)
- `FullName: string`
- `UserId: string` (FK → ApplicationUser)
- `DocumentType: DocumentType` (enum)
- `Identification: string`
- `Address: string`
- `BirthDate: DateTime`
- `ProfessionalProfileId: int` (FK → ProfessionalProfile)
- `Country: string`
- `City: string`
- `Gender: Gender` (enum)
- `StatusId: int` (FK → StatusCoder)
- Navigation collections: `SoftSkills`, `TechnicalSkills`, `Feedback`, `ClanCoders`, `Attendances`, `FaceCollections`, `Notifications`

### Clan
- `Id: int` (PK)
- `Name: string`
- `Description: string`
- Navigation collections: `ClanCoders` (many‑to‑many with Coder), `ClanTeamLeaders` (many‑to‑many with TeamLeader)

### ClanCoder (junction table)
- `ClanId: int` (FK → Clan)
- `CoderId: int` (FK → Coder)
- Composite PK `(ClanId, CoderId)`

### ClanTeamLeader (junction table)
- `ClanId: int` (FK → Clan)
- `TeamLeaderId: int` (FK → TeamLeader)
- Composite PK `(ClanId, TeamLeaderId)`

### Class
- `Id: int` (PK)
- `Name: string`
- `Description: string`
- `ClassTypeId: int` (FK → ClassType)
- `DayId: int` (FK → Day)
- `StartTime: TimeSpan`
- `EndTime: TimeSpan`
- Navigation collections: `Attendances`

### ClassType
- `Id: int` (PK)
- `Name: string`
- `Description: string`

### Day
- `Id: int` (PK)
- `Name: string` (e.g., Monday)

### CoderSoftSkill (junction)
- `CoderId: int` (FK → Coder)
- `SoftSkillId: int` (FK → SoftSkill)
- Composite PK `(CoderId, SoftSkillId)`

### CoderTechnicalSkill (junction)
- `CoderId: int` (FK → Coder)
- `TechnicalSkillId: int` (FK → TechnicalSkill)
- Composite PK `(CoderId, TechnicalSkillId)`

### SoftSkill
- `Id: int` (PK)
- `Name: string`
- `Description: string`

### TechnicalSkill
- `Id: int` (PK)
- `Name: string`
- `Description: string`
- `Level: TechnicalSkillLevel` (enum)

### Feedback
- `Id: int` (PK)
- `CoderId: int?` (FK → Coder, optional)
- `TeamLeaderId: int?` (FK → TeamLeader, optional)
- `FeedbackType: FeedbackType` (enum)
- `Content: string`
- Navigation properties: `Coder`, `TeamLeader`

### Notification
- `Id: int` (PK)
- `UserId: string` (FK → ApplicationUser)
- `Message: string`
- `Type: NotificationType` (enum)
- `IsRead: bool`
- `CreatedAt: DateTime`

### ProfessionalProfile
- `Id: int` (PK)
- `Title: string`
- `Description: string`
- `Specialties` (collection of `Specialty` via `TeamLeaderSpecialty`)

### Specialty
- `Id: int` (PK)
- `Name: string`
- `Level: TeamLeaderSpecialtyLevel` (enum)

### StatusCoder
- `Id: int` (PK)
- `Name: string`

### TeamLeader
- `Id: int` (PK)
- `FullName: string`
- `UserId: string` (FK → ApplicationUser)
- Navigation collections: `ClanTeamLeaders`, `Specialties`

### TeamLeaderSpecialty (junction)
- `TeamLeaderId: int` (FK → TeamLeader)
- `SpecialtyId: int` (FK → Specialty)
- Composite PK `(TeamLeaderId, SpecialtyId)`

## Enums (Values)
### AttendanceStatus
- `Present`
- `Absent`
- `Justified`
- `Late`
- `Excused`

### DocumentType
- `DNI`
- `Passport`
- `Other`
- `Unknown`

### Gender
- `Male`
- `Female`
- `Other`
- `Unknown`

### NotificationType
- `Info`
- `Warning`
- `Error`

### TeamLeaderSpecialtyLevel
- `Junior`
- `Mid`
- `Senior`
- `Lead`

### TechnicalSkillLevel
- `Beginner`
- `Intermediate`
- `Advanced`
- `Expert`

## Relationships Overview
- **Coder ↔ Clan**: many‑to‑many via `ClanCoder`.
- **TeamLeader ↔ Clan**: many‑to‑many via `ClanTeamLeader`.
- **Coder ↔ SoftSkill**: many‑to‑many via `CoderSoftSkill`.
- **Coder ↔ TechnicalSkill**: many‑to‑many via `CoderTechnicalSkill`.
- **Clan ↔ Attendance**: one‑to‑many (a Clan has many Attendances).
- **Class ↔ Attendance**: one‑to‑many.
- **Coder ↔ Attendance**: one‑to‑many.
- **Coder ↔ Feedback**: one‑to‑many (optional TeamLeader side).
- **TeamLeader ↔ Feedback**: one‑to‑many (optional Coder side).
- **TeamLeader ↔ Specialty**: many‑to‑many via `TeamLeaderSpecialty`.
- **ProfessionalProfile ↔ Specialty**: one‑to‑many (profile aggregates specialties).
- **Notification ↔ ApplicationUser**: many‑to‑one (each notification belongs to a user).
- **Class ↔ ClassType / Day**: many‑to‑one (each Class has a ClassType and a Day).


## Architecture
RiwiLens follows **Clean Architecture** combined with **Domain‑Driven Design (DDD)** principles:

- **API Layer (`src/RiwiLens.Api`)** – Controllers, Middleware, Extensions, Program.cs. Handles HTTP requests and responses.
- **Application Layer (`src/RiwiLens.Application`)** – Services, Interfaces, DTOs, Commands, Queries, Validators. Contains use‑cases and business rules.
- **Domain Layer (`src/RiwiLens.Domain`)** – Entities, Enums, Value Objects, Exceptions. Represents the core business model and invariants.
- **Infrastructure Layer (`src/RiwiLens.Infrastructure`)** – EF Core `DbContext`, Repositories, Configurations, Identity, Migrations, external service integrations.

### Folder Overview
- **src/RiwiLens.Api** – Entry point, routing, API contracts.
- **src/RiwiLens.Application** – Application services, validation, command/query handling.
- **src/RiwiLens.Domain** – Rich domain model (entities, value objects, aggregates) following DDD.
- **src/RiwiLens.Infrastructure**
  - **Configurations** – EF Core relationship mappings, database configuration.
  - **Services** – Integrations with external systems (e.g., email, storage).
  - **Identity** – ASP.NET Core Identity setup for user management.
  - **Migrations** – Database schema evolution scripts.
- **tests/** – Unit, Integration, and Fixture tests for all layers.
- **docs/** – Architecture diagrams, API specifications, and additional documentation.


## License
This project is licensed under the MIT License. See the `LICENSE` file for details.