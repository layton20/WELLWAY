using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wellway.Domain.Entities;

namespace Wellway.Infrastructure.Persistence.Configurations;

public sealed class PatientConfiguration : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder.ToTable("Patients");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.HospitalPatientId)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(p => p.HospitalPatientId).IsUnique();

        builder.Property(p => p.NhsNumber)
            .HasMaxLength(10);

        builder.HasIndex(p => p.NhsNumber)
            .IsUnique()
            .HasFilter("[NhsNumber] IS NOT NULL");

        builder.Property(p => p.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.PreferredName)
            .HasMaxLength(100);

        builder.Property(p => p.DateOfBirth)
            .IsRequired()
            .HasColumnType("date");

        builder.Property(p => p.BiologicalSex)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(p => p.GenderIdentity)
            .HasMaxLength(100);

        builder.Property(p => p.Pronouns)
            .HasMaxLength(50);

        builder.Property(p => p.PhoneNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(p => p.Email)
            .HasMaxLength(255);

        builder.Property(p => p.EmergencyContactName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.EmergencyContactPhone)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(p => p.EmergencyContactRelationship)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.BloodType)
            .IsRequired()
            .HasConversion<int>();

        builder.OwnsOne(p => p.Address, address =>
        {
            address.Property(a => a.Line1)
                .HasColumnName("Address_Line1")
                .IsRequired()
                .HasMaxLength(200);

            address.Property(a => a.Line2)
                .HasColumnName("Address_Line2")
                .HasMaxLength(200);

            address.Property(a => a.City)
                .HasColumnName("Address_City")
                .IsRequired()
                .HasMaxLength(100);

            address.Property(a => a.County)
                .HasColumnName("Address_County")
                .HasMaxLength(100);

            address.Property(a => a.PostCode)
                .HasColumnName("Address_PostCode")
                .IsRequired()
                .HasMaxLength(10);

            address.Property(a => a.Country)
                .HasColumnName("Address_Country")
                .IsRequired()
                .HasMaxLength(100);
        });

        builder.Property(p => p.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(p => p.DeletedAt);

        builder.Property(p => p.CreatedAt).IsRequired();
        builder.Property(p => p.UpdatedAt).IsRequired();

        builder.Property(p => p.CreatedBy)
            .IsRequired()
            .HasMaxLength(450);

        builder.HasMany(p => p.Appointments)
            .WithOne(a => a.Patient)
            .HasForeignKey(a => a.PatientId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
