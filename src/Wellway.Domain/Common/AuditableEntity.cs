namespace Wellway.Domain.Common;

public abstract class AuditableEntity : BaseEntity
{
    public string CreatedBy { get; private set; } = string.Empty;
    public bool IsDeleted { get; private set; }
    public DateTimeOffset? DeletedAt { get; private set; }

    protected AuditableEntity() { }

    protected AuditableEntity(string createdBy)
    {
        CreatedBy = createdBy;
    }

    protected void SoftDelete()
    {
        IsDeleted = true;
        DeletedAt = DateTimeOffset.UtcNow;
        UpdateTimestamp();
    }

    protected void Restore()
    {
        IsDeleted = false;
        DeletedAt = null;
        UpdateTimestamp();
    }
}
