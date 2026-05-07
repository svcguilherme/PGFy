using CareerHub.SharedKernel;

namespace CareerHub.Domain.Financeiro.Entities;

public record DespesaCriadaEvent(Guid DespesaId) : IDomainEvent;
