using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wellway.Domain.Entities;
using Wellway.Domain.Enums;

namespace Wellway.Infrastructure.Persistence.Configurations;

public sealed class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.ToTable("Appointments");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.PatientId).IsRequired();

        builder.Property(a => a.StaffId).IsRequired();

        builder.HasOne(a => a.Staff)
            .WithMany(s => s.Appointments)
            .HasForeignKey(a => a.StaffId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(a => a.DepartmentId).IsRequired();

        builder.HasOne(a => a.Department)
            .WithMany(d => d.Appointments)
            .HasForeignKey(a => a.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(a => a.AppointmentType)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(a => a.Status)
            .IsRequired()
            .HasConversion<int>()
            .HasDefaultValue(AppointmentStatus.Scheduled);

        builder.Property(a => a.ScheduledAt)
            .IsRequired()
            .HasColumnType("datetimeoffset");

        builder.Property(a => a.DurationInMinutes).IsRequired();

        builder.Property(a => a.Room)
            .HasMaxLength(50);

        builder.Property(a => a.ReasonForVisit)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(a => a.CancellationReason)
            .HasMaxLength(500);

        builder.Property(a => a.Notes)
            .HasMaxLength(2000);

        builder.Property(a => a.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(a => a.DeletedAt);

        builder.Property(a => a.CreatedAt).IsRequired();
        builder.Property(a => a.UpdatedAt).IsRequired();

        builder.Property(a => a.CreatedBy)
            .IsRequired()
            .HasMaxLength(450);
    }
}
