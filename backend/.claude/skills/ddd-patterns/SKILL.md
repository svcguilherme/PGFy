---
name: ddd-patterns
description: Use this skill when creating or modifying Domain entities, Value Objects, Domain Events, Aggregates, or Repository interfaces. Trigger on: "criar entidade", "value object", "aggregate", "domain event", "repositório", "bounded context".
---

# DDD Patterns — CareerHub

## BaseEntity e AuditableEntity

```csharp
// SharedKernel/BaseEntity.cs
public abstract class BaseEntity
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    private readonly List<IDomainEvent> _domainEvents = [];
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    protected void AddDomainEvent(IDomainEvent @event) => _domainEvents.Add(@event);
    public void ClearDomainEvents() => _domainEvents.Clear();
}

// SharedKernel/AuditableEntity.cs
public abstract class AuditableEntity : BaseEntity
{
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; private set; }
    public Guid CreatedBy { get; private set; }
    public void SetUpdated() => UpdatedAt = DateTime.UtcNow;
}
```

## Entidades do Domínio

### Estudo (Módulo Estudos)
```csharp
public class Estudo : AuditableEntity
{
    public string Titulo { get; private set; }
    public DiaDaSemana DiaDaSemana { get; private set; }
    public TimeOnly HoraInicio { get; private set; }
    public TimeOnly HoraFim { get; private set; }
    public double HorasTotais => (HoraFim - HoraInicio).TotalHours;
    public string? Descricao { get; private set; }
    public Guid UsuarioId { get; private set; }

    private Estudo() { } // EF Core

    public static Result<Estudo> Create(string titulo, DiaDaSemana dia,
        TimeOnly inicio, TimeOnly fim, Guid usuarioId)
    {
        if (fim <= inicio)
            return Result.Failure<Estudo>("Hora de fim deve ser após hora de início.");
        if (string.IsNullOrWhiteSpace(titulo))
            return Result.Failure<Estudo>("Título é obrigatório.");

        var estudo = new Estudo
        {
            Titulo = titulo, DiaDaSemana = dia,
            HoraInicio = inicio, HoraFim = fim, UsuarioId = usuarioId
        };
        estudo.AddDomainEvent(new EstudoCriadoEvent(estudo.Id));
        return Result.Success(estudo);
    }
}
```

### Despesa (Módulo Financeiro)
```csharp
public class Despesa : AuditableEntity
{
    public string Descricao { get; private set; }
    public decimal Valor { get; private set; }
    public DateTime DataPrevista { get; private set; }
    public bool Pago { get; private set; }
    public CategoriaDespesa Categoria { get; private set; }
    public Guid UsuarioId { get; private set; }

    public static Result<Despesa> Create(string descricao, decimal valor,
        DateTime dataPrevista, CategoriaDespesa categoria, Guid usuarioId)
    {
        if (valor <= 0) return Result.Failure<Despesa>("Valor deve ser positivo.");
        return Result.Success(new Despesa { /* ... */ });
    }

    public void MarcarComoPago() => Pago = true;
}
```

### Recebivel (Módulo Financeiro)
```csharp
public class Recebivel : AuditableEntity
{
    public string Descricao { get; private set; }
    public decimal ValorPrevisto { get; private set; }
    public DateTime DataPrevista { get; private set; }
    public bool Recebido { get; private set; }
    public Guid UsuarioId { get; private set; }
}
```

## Value Objects

```csharp
// Exemplo: FluxoMensal (calculado, não persistido separado)
public record FluxoMensal(int Ano, int Mes, decimal TotalRecebiveis,
    decimal TotalDespesas)
{
    public decimal Saldo => TotalRecebiveis - TotalDespesas;
    public bool Positivo => Saldo >= 0;
}
```

## Repository Interfaces (no Domain)

```csharp
// Sempre interface no Domain, implementação na Infrastructure
public interface IEstudoRepository
{
    Task<Estudo?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<Estudo>> GetByUsuarioAsync(Guid usuarioId, ct);
    Task<IEnumerable<Estudo>> GetByDiaAsync(Guid usuarioId, DiaDaSemana dia, ct);
    Task AddAsync(Estudo estudo, ct);
    Task UpdateAsync(Estudo estudo, ct);
    Task DeleteAsync(Guid id, ct);
}
```

## Enums de Domínio

```csharp
public enum DiaDaSemana { Segunda, Terca, Quarta, Quinta, Sexta, Sabado, Domingo }
public enum CategoriaDespesa { Alimentacao, Transporte, Educacao, Saude, Lazer, Outros }
public enum RoleUsuario { Admin, User }
```
