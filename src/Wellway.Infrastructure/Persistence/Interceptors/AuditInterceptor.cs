using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Wellway.Domain.Auditing;
using Wellway.Domain.Common;

namespace Wellway.Infrastructure.Persistence.Interceptors;

public sealed class AuditInterceptor(IHttpContextAccessor httpContextAccessor) : SaveChangesInterceptor
{
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
            CreateAuditLogs(eventData.Context);

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void CreateAuditLogs(DbContext context)
    {
        var changedBy = httpContextAccessor.HttpContext?
            .User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "system";

        var auditEntries = new List<AuditLog>();

        foreach (var entry in context.ChangeTracker.Entries())
        {
            if (entry.Entity is AuditLog) continue;
            if (entry.Entity is not BaseEntity entity) continue;
            if (entry.State is not (EntityState.Added or EntityState.Modified or EntityState.Deleted)) continue;

            var entityName = entry.Entity.GetType().Name;
            var entityId = entity.Id;

            string? oldValues = null;
            string? newValues = null;
            AuditAction action;

            if (entry.State == EntityState.Added)
            {
                action = AuditAction.Created;
                newValues = SerializeProperties(entry.CurrentValues);
            }
            else if (entry.State == EntityState.Modified)
            {
                oldValues = SerializeProperties(entry.OriginalValues);
                newValues = SerializeProperties(entry.CurrentValues);

                var isDeletedProperty = entry.Properties
                    .FirstOrDefault(p => p.Metadata.Name == nameof(AuditableEntity.IsDeleted));

                if (isDeletedProperty is { IsModified: true })
                {
                    action = (bool)isDeletedProperty.CurrentValue!
                        ? AuditAction.SoftDeleted
                        : AuditAction.Restored;
                }
                else
                {
                    action = AuditAction.Updated;
                }
            }
            else
            {
                oldValues = SerializeProperties(entry.OriginalValues);
                action = AuditAction.SoftDeleted;
            }

            auditEntries.Add(new AuditLog(entityName, entityId, action, changedBy, oldValues, newValues));
        }

        context.Set<AuditLog>().AddRange(auditEntries);
    }

    private static string SerializeProperties(Microsoft.EntityFrameworkCore.ChangeTracking.PropertyValues values)
    {
        var dict = values.Properties
            .ToDictionary(p => p.Name, p => values[p]);

        return JsonSerializer.Serialize(dict);
    }
}
