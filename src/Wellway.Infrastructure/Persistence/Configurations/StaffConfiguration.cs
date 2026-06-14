using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wellway.Domain.Entities;

namespace Wellway.Infrastructure.Persistence.Configurations;

public sealed class StaffConfiguration : IEntityTypeConfiguration<Staff>
{
    public void Configure(EntityTypeBuilder<Staff> builder)
    {
        builder.ToTable("StaffMembers");

        builder.HasKey(s => s.Id);

        builder.Ignore(s => s.FullName);

        builder.Property(s => s.UserId)
            .IsRequired()
            .HasMaxLength(450);

        builder.HasIndex(s => s.UserId).IsUnique();

        builder.Property(s => s.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.JobTitle)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(s => s.StaffRole)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(s => s.DepartmentId).IsRequired();

        builder.HasOne(s => s.Department)
            .WithMany(d => d.StaffMembers)
            .HasForeignKey(s => s.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(s => s.PhoneExtension)
            .HasMaxLength(10);

        builder.Property(s => s.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(s => s.CreatedAt).IsRequired();
        builder.Property(s => s.UpdatedAt).IsRequired();
    }
}
