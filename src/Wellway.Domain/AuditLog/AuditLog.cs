namespace Wellway.Domain.Auditing;

public sealed class AuditLog
{
    public Guid Id { get; private set; }
    public string EntityName { get; private set; } = string.Empty;
    public Guid EntityId { get; private set; }
    public AuditAction Action { get; private set; }
    public string ChangedBy { get; private set; } = string.Empty;
    public DateTimeOffset ChangedAt { get; private set; }
    public string? OldValues { get; private set; }
    public string? NewValues { get; private set; }

    private AuditLog() { }

    public AuditLog(
        string entityName,
        Guid entityId,
        AuditAction action,
        string changedBy,
        string? oldValues,
        string? newValues)
    {
        Id = Guid.NewGuid();
        EntityName = entityName;
        EntityId = entityId;
        Action = action;
        ChangedBy = changedBy;
        ChangedAt = DateTimeOffset.UtcNow;
        OldValues = oldValues;
        NewValues = newValues;
    }
}
