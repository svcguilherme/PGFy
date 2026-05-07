namespace CareerHub.SharedKernel;

public abstract class AuditableEntity : BaseEntity
{
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; private set; }
    public Guid CreatedBy { get; private set; }

    protected void SetCreatedBy(Guid userId) => CreatedBy = userId;
    public void SetUpdated() => UpdatedAt = DateTime.UtcNow;
}
