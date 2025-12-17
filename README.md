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
| `/api/auth/refresh` | POST | Uses a refresh token (also stored in a cookie) to issue a new JWT without re‑authenticating. |
| `/api/auth/logout` | POST | Clears authentication cookies, effectively logging the user out. |
| `/api/auth/me` | GET | Returns the full profile of the authenticated user: `id`, `uuid`, `email`, `firstName`, `lastName`, `role`, `documentType`, `documentNumber`, `gender`, `isActive`, `profileImageUrl`, etc. |

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
