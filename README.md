# Bookify

A modern booking platform built with **.NET** using **Domain-Driven Design (DDD)** and **Clean Architecture**.

---

## 📌 About the Project

**Bookify** is designed to showcase enterprise-level software design patterns in .NET. It enforces strict separation of concerns through layered architecture, keeping the core domain logic completely isolated from framework dependencies, database concerns, and external services.

### Key Architectural Concepts
* **Domain-Driven Design (DDD):** Entities, Value Objects, Aggregates, Domain Events, and Enums representing the core domain rules.
* **Clean Architecture:** Strict dependency flow pointing inwards (`Domain` $\rightarrow$ `Application` $\rightarrow$ `Infrastructure` $\rightarrow$ `Api`).
* **CQRS Pattern:** Separation of Read and Write operations using MediatR.
* **Result Pattern:** Explicit functional error handling using custom `Result` and `Error` types instead of throwing costly runtime exceptions.

---

## 🛠 Tech Stack

* **Framework:** .NET 8 Web API
* **Database:** PostgreSQL (via Entity Framework Core)
* **Containerization:** Docker & Docker Compose
* **ORM & Migrations:** EF Core Code-First and Dapper
* **API Documentation:** OpenAPI / Swagger UI
