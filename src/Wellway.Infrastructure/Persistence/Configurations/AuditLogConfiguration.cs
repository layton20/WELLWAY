using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wellway.Domain.Auditing;

namespace Wellway.Infrastructure.Persistence.Configurations;

public sealed class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.ToTable("AuditLogs");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.EntityName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.EntityId).IsRequired();

        builder.Property(a => a.Action)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(a => a.ChangedBy)
            .IsRequired()
            .HasMaxLength(450);

        builder.Property(a => a.ChangedAt).IsRequired();

        builder.Property(a => a.OldValues)
            .HasColumnType("nvarchar(max)");

        builder.Property(a => a.NewValues)
            .HasColumnType("nvarchar(max)");
    }
}
