# Bookify

A booking platform for renting apartments, built with **.NET 8** using **Domain-Driven Design (DDD)** and **Clean Architecture**.

Bookify is a reference project for enterprise-style design in .NET. The core domain is isolated from frameworks, databases and external services, and every layer is covered by its own kind of test.

---

## 📌 Architecture

The solution follows Clean Architecture: every project reference points **inward**, toward the domain.

```
            ┌──────────────┐
            │ Bookify.Api  │   composition root (ASP.NET Core)
            └──┬────────┬──┘
               │        │
               ▼        ▼
┌─────────────────────┐   ┌────────────────────────┐
│ Bookify.Application │◄──│ Bookify.Infrastructure │
└──────────┬──────────┘   └────────────────────────┘
           ▼
┌─────────────────────┐
│   Bookify.Domain    │   no project references
└─────────────────────┘
```

| Project | Responsibility |
|---|---|
| **Bookify.Domain** | Entities, aggregates (`Apartment`, `Booking`, `User`), value objects, domain events, domain errors and the `Result` type. No framework dependencies. |
| **Bookify.Application** | Use cases as CQRS commands and queries (MediatR), FluentValidation validators, pipeline behaviors (logging, validation, query caching), and the abstractions Infrastructure implements. |
| **Bookify.Infrastructure** | EF Core + PostgreSQL persistence, repositories, Keycloak authentication, permission-based authorization, Redis caching, the transactional outbox, API versioning and health checks. |
| **Bookify.Api** | Controllers, exception-handling middleware, Swagger, Serilog, and wiring of all layers. |

### Key concepts

- **Domain-Driven Design:** aggregates enforce their own invariants and raise domain events.
- **CQRS:** writes go through EF Core; reads use Dapper with hand-written SQL.
- **Result pattern:** expected failures are returned as `Result` / `Error` values instead of exceptions.
- **Transactional outbox:** domain events are saved in the same transaction as the change and published by a Quartz background job.
- **Permission-based authorization:** roles and permissions live in the database and are cached in Redis; endpoints are protected with `[HasPermission]`.

---

## 🛠 Tech Stack

| Area | Technology |
|---|---|
| Framework | .NET 8, ASP.NET Core Web API |
| Messaging | MediatR |
| Validation | FluentValidation |
| Persistence | PostgreSQL, EF Core (writes, migrations), Dapper (reads) |
| Identity | Keycloak (JWT bearer) |
| Caching | Redis |
| Background jobs | Quartz.NET |
| Logging | Serilog + Seq |
| API docs | Swagger / OpenAPI, API versioning |
| Containers | Docker, Docker Compose |
| Testing | xUnit, FluentAssertions, NSubstitute, Testcontainers, NetArchTest |

---

## 📡 API

All routes are versioned under `api/v1`.

| Method | Route | Auth | Description |
|---|---|---|---|
| `POST` | `/api/v1/user/register` | Anonymous | Register a user (creates the Keycloak identity and the local user) |
| `POST` | `/api/v1/user/login` | Anonymous | Log in and receive an access token |
| `GET` | `/api/v1/user/me` | `users:read` permission | Get the logged-in user |
| `GET` | `/api/v1/apartments?startDate=&endDate=` | Authenticated | Search apartments available in a date range |
| `GET` | `/api/v1/bookings?id=` | None | Get a booking by id |
| `POST` | `/api/v1/bookings` | None | Reserve a booking |

---

## 🧪 Testing

| Project | Kind | What it covers |
|---|---|---|
| `Bookify.Domain.UnitTests` | Unit | Domain entities and business rules |
| `Bookify.Application.UnitTests` | Unit | Command/query handlers with NSubstitute mocks |
| `Bookify.Application.IntegrationTests` | Integration | Handlers against real PostgreSQL, Redis and Keycloak (Testcontainers) |
| `Bookify.Api.FunctionalTests` | End-to-end | HTTP endpoints through `WebApplicationFactory` (Testcontainers) |
| `Bookify.ArchitectureTests` | Architecture | Layer dependencies and conventions (NetArchTest) |

---

## 📄 License

This project is licensed under the [MIT License](LICENSE).
