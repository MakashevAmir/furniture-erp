# FurnitureERP

> **Note:** This project is a work in progress and is part of my diploma thesis.

## Description

FurnitureERP is a production information system for furniture manufacturing. It helps manage products, materials, employees, orders, and cost calculations.

## Technologies

- **.NET 8** - Backend framework
- **Blazor Server** - Interactive web UI
- **Radzen Blazor** - UI component library
- **Entity Framework Core** - ORM for data access
- **SQLite** - Database
- **MediatR** - CQRS pattern implementation
- **AutoMapper** - Object mapping
- **FluentValidation** - Input validation
- **ASP.NET Core Identity** - Authentication

## Architecture

The project follows **Clean Architecture** and **Domain-Driven Design (DDD)** principles:

```
FurnitureERP.Domain        - Domain entities, aggregates, events, repositories interfaces
FurnitureERP.Application   - Use cases, DTOs, commands, queries, validators
FurnitureERP.Infrastructure - EF Core, repository implementations, database
FurnitureERP.Web           - Blazor Server UI, Radzen components
FurnitureERP.Tests         - Unit and integration tests
```

## Features

- Product management with Bill of Materials (BOM)
- Material inventory tracking
- Employee management with hourly rates
- Order processing with status workflow
- Automatic cost calculation
- Dashboard with statistics

## Getting Started

1. Clone the repository
2. Run `dotnet restore`
3. Run `dotnet run --project FurnitureERP`
4. Open `https://localhost:5001`
5. Login with `admin` / `admin`
