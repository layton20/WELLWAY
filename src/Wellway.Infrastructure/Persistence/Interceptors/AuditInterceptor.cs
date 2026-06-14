using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
                newValues = SerializeEntry(entry, useOriginal: false);
            }
            else if (entry.State == EntityState.Modified)
            {
                oldValues = SerializeEntry(entry, useOriginal: true);
                newValues = SerializeEntry(entry, useOriginal: false);

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
                oldValues = SerializeEntry(entry, useOriginal: true);
                action = AuditAction.SoftDeleted;
            }

            auditEntries.Add(new AuditLog(entityName, entityId, action, changedBy, oldValues, newValues));
        }

        context.Set<AuditLog>().AddRange(auditEntries);
    }

    private static string SerializeEntry(EntityEntry entry, bool useOriginal)
    {
        var values = useOriginal ? entry.OriginalValues : entry.CurrentValues;
        var dict = new Dictionary<string, object?>();

        foreach (var property in values.Properties)
        {
            dict[property.Name] = values[property];
        }

        // Owned entity navigations (e.g. Patient.Address) are tracked as separate
        // EntityEntries and do not appear in the owning entity's PropertyValues.
        // Walk references and merge owned entity scalars into the same dictionary.
        foreach (var reference in entry.References)
        {
            var targetEntry = reference.TargetEntry;
            if (targetEntry is null || !targetEntry.Metadata.IsOwned()) continue;

            var ownedValues = useOriginal ? targetEntry.OriginalValues : targetEntry.CurrentValues;
            foreach (var property in ownedValues.Properties)
            {
                dict[property.Name] = ownedValues[property];
            }
        }

        return JsonSerializer.Serialize(dict);
    }
}
