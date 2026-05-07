using CareerHub.SharedKernel;

namespace CareerHub.Domain.Estudos.Entities;

public record EstudoCriadoEvent(Guid EstudoId) : IDomainEvent;
