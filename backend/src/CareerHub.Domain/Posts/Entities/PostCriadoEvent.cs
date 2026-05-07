using CareerHub.SharedKernel;

namespace CareerHub.Domain.Posts.Entities;

public record PostCriadoEvent(Guid PostId) : IDomainEvent;
