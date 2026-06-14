namespace Wellway.Domain.Auditing;

public enum AuditAction
{
    Created = 0,
    Updated = 1,
    SoftDeleted = 2,
    Restored = 3,
}
