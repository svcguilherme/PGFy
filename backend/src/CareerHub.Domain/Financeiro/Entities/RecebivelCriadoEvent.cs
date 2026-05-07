using CareerHub.SharedKernel;

namespace CareerHub.Domain.Financeiro.Entities;

public record RecebivelCriadoEvent(Guid RecebivelId) : IDomainEvent;
