---
name: dotnet-conventions
description: Use this skill when writing any C# code, creating new files, setting up dependency injection, writing EF Core configurations, creating migrations, or following C# coding standards. Trigger on: "criar arquivo", "novo serviço", "ef core", "migration", "di", "injeção de dependência", "c#".
---

# .NET Conventions — CareerHub

## C# Code Style

```csharp
// ✅ Primary constructors (C# 12)
public class EstudoService(IEstudoRepository repo, IMapper mapper) { }

// ✅ Collection expressions
List<string> items = [];
var ids = new List<Guid> { id1, id2 };

// ✅ Pattern matching
if (result is { IsSuccess: true, Value: var estudo })
    return Ok(mapper.Map<EstudoDto>(estudo));

// ✅ Records para DTOs imutáveis
public record CreateEstudoDto(string Titulo, DiaDaSemana Dia,
    TimeOnly HoraInicio, TimeOnly HoraFim, string? Descricao);

// ❌ NUNCA var para tipos não óbvios
var x = GetSomething(); // ruim
EstudoDto estudo = GetSomething(); // bom
```

## EF Core — Configuração por Fluent API

```csharp
// Infrastructure/Persistence/Configurations/EstudoConfiguration.cs
public class EstudoConfiguration : IEntityTypeConfiguration<Estudo>
{
    public void Configure(EntityTypeBuilder<Estudo> builder)
    {
        builder.ToTable("estudos");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Titulo).HasMaxLength(200).IsRequired();
        builder.Property(e => e.DiaDaSemana).HasConversion<string>();
        builder.HasIndex(e => new { e.UsuarioId, e.DiaDaSemana });
    }
}
```

## Result Pattern (SharedKernel)

```csharp
// SharedKernel/Result.cs
public class Result<T>
{
    public bool IsSuccess { get; }
    public T? Value { get; }
    public string? Error { get; }

    private Result(bool isSuccess, T? value, string? error)
    { IsSuccess = isSuccess; Value = value; Error = error; }

    public static Result<T> Success(T value) => new(true, value, null);
    public static Result<T> Failure(string error) => new(false, default, error);
}

public static class Result
{
    public static Result<T> Success<T>(T value) => Result<T>.Success(value);
    public static Result<T> Failure<T>(string error) => Result<T>.Failure(error);
}
```

## FluentValidation

```csharp
// Application/Estudos/Validators/CreateEstudoValidator.cs
public class CreateEstudoValidator : AbstractValidator<CreateEstudoDto>
{
    public CreateEstudoValidator()
    {
        RuleFor(x => x.Titulo)
            .NotEmpty().WithMessage("Título é obrigatório.")
            .MaximumLength(200).WithMessage("Título não pode exceder 200 caracteres.");

        RuleFor(x => x.HoraFim)
            .GreaterThan(x => x.HoraInicio)
            .WithMessage("Hora de fim deve ser após hora de início.");
    }
}
```

## AutoMapper Profile

```csharp
// Application/Estudos/Mappings/EstudoProfile.cs
public class EstudoProfile : Profile
{
    public EstudoProfile()
    {
        CreateMap<Estudo, EstudoDto>()
            .ForMember(d => d.HorasTotais, o => o.MapFrom(s => s.HorasTotais));
        CreateMap<CreateEstudoDto, CreateEstudoCommand>();
    }
}
```

## Estrutura de Arquivos por Módulo (Application)

```
Application/
└── Estudos/
    ├── Commands/
    │   ├── CreateEstudo/
    │   │   ├── CreateEstudoCommand.cs
    │   │   └── CreateEstudoHandler.cs
    │   └── DeleteEstudo/
    │       ├── DeleteEstudoCommand.cs
    │       └── DeleteEstudoHandler.cs
    ├── Queries/
    │   ├── GetEstudosByDia/
    │   └── GetEstudosByUsuario/
    ├── DTOs/
    │   ├── EstudoDto.cs
    │   └── CreateEstudoDto.cs
    ├── Validators/
    │   └── CreateEstudoValidator.cs
    └── Mappings/
        └── EstudoProfile.cs
```

## Migrations

```bash
# Sempre nomear com contexto claro
dotnet ef migrations add AddEstudosTable --project src/CareerHub.Infrastructure --startup-project src/CareerHub.API
dotnet ef database update --project src/CareerHub.Infrastructure --startup-project src/CareerHub.API
```

## Global Exception Handling (Middleware)

```csharp
app.UseExceptionHandler(appError => appError.Run(async ctx => {
    ctx.Response.StatusCode = 500;
    ctx.Response.ContentType = "application/json";
    await ctx.Response.WriteAsJsonAsync(new {
        error = "Ocorreu um erro interno. Tente novamente."
        // NUNCA expor stack trace em produção
    });
}));
```
