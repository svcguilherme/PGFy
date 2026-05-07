# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

> Inherits all rules from `/CLAUDE.md`

## Stack
- .NET 8 / C# 12
- ASP.NET Core Web API (Controllers + Minimal API hybrid)
- Entity Framework Core 8 + Sql Server
- ASP.NET Core Identity + JWT Bearer
- MediatR (CQRS) + FluentValidation + AutoMapper
- xUnit + Moq + FluentAssertions

## Commands

```bash
# Build & run
dotnet restore
dotnet build
dotnet run --project src/CareerHub.API

# Tests
dotnet test
dotnet test --filter "FullyQualifiedName~CreateEstudoHandlerTests"  # single test class

# EF Core migrations (always specify both projects)
dotnet ef migrations add <MigrationName> --project src/CareerHub.Infrastructure --startup-project src/CareerHub.API
dotnet ef database update               --project src/CareerHub.Infrastructure --startup-project src/CareerHub.API
```

## Layer Structure

```
CareerHub.Domain/         → Entities, Value Objects, Domain Events, Repository interfaces
CareerHub.Application/    → Commands/Queries, DTOs, Validators, AutoMapper profiles
CareerHub.Infrastructure/ → EF Core, Repository impls, Identity, JWT, external HttpClients
CareerHub.API/            → Controllers, Middleware, DI, Program.cs
CareerHub.SharedKernel/   → Result<T>, BaseEntity, AuditableEntity, shared enums
```

## Bounded Contexts

| Module     | Responsibility                               | Persistence       |
|------------|----------------------------------------------|-------------------|
| Identity   | Users, Roles, JWT, Refresh Token             | SQL Server        |
| Estudos    | Weekly study tasks, hours calculation        | SQL Server        |
| Posts      | User publications CRUD                       | SQL Server        |
| Financeiro | Expenses, Receivables, monthly cash flow     | SQL Server        |
| Conteudo   | RSS feed aggregation (.NET / Angular blogs)  | None — cache only |

## Application Layer Convention

One folder per use case inside the bounded context folder:

```
Application/Estudos/
├── Commands/
│   └── CreateEstudo/
│       ├── CreateEstudoCommand.cs   → record implementing IRequest<Result<EstudoDto>>
│       └── CreateEstudoHandler.cs  → IRequestHandler implementation
├── Queries/GetEstudosByDia/
├── DTOs/                            → records (immutable)
├── Validators/                      → AbstractValidator<TDto>
└── Mappings/                        → Profile subclasses
```

## Required Patterns

### Result Pattern
```csharp
// SharedKernel — use everywhere, never throw for business logic
return Result.Failure<EstudoDto>("Hora de início deve ser anterior à hora de fim.");
return Result.Success(mapper.Map<EstudoDto>(estudo));
```

### Domain Entities
- Inherit `AuditableEntity` (`CreatedAt`, `UpdatedAt`, `CreatedBy`)
- All properties `private set` — mutation only through domain methods
- Private parameterless constructor for EF Core
- Static `Create(...)` factory method that returns `Result<T>` and raises domain events

### Controllers
```csharp
[Authorize]
[ApiController, Route("api/v1/[controller]")]
public class EstudosController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateEstudoDto dto)
    {
        var result = await mediator.Send(new CreateEstudoCommand(dto, UserId));
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    // UserId helper — every authorized controller needs this
    protected Guid UserId =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}
```

### C# 12 Conventions
- Primary constructors for services and handlers (not entities)
- `record` for all DTOs and Commands/Queries
- Collection expressions: `List<T> items = [];`
- Explicit types preferred over `var` when the type isn't obvious from the right-hand side

## Identity & JWT

- Roles: `Admin` (full access), `User` (filtered by `UsuarioId`)
- Access Token expiry: 15 min; Refresh Token expiry: 7 days
- Refresh Token stored in the database on `ApplicationUser`

## Conteudo Module

- Fetches RSS from DEV.to, .NET Blog, Angular Blog
- `IMemoryCache` with 1-hour TTL — never persist to database
- Returns `IEnumerable<ConteudoItemDto>` directly from cache/fetch

## Reavaliar todas as skills 
- Poderia realivar todas as skills e ver o que ficou errado no projeto