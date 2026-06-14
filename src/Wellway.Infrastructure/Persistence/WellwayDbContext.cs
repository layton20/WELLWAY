using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Wellway.Application.Identity;
using Wellway.Application.Interfaces;
using Wellway.Domain.Auditing;
using Wellway.Domain.Entities;

namespace Wellway.Infrastructure.Persistence;

public sealed class WellwayDbContext(DbContextOptions<WellwayDbContext> options)
    : IdentityDbContext<WellwayIdentityUser>(options), IWellwayDbContext
{
    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<Staff> StaffMembers => Set<Staff>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    public async Task<long> GetNextPatientSequenceValueAsync(CancellationToken cancellationToken = default)
        => await Database
            .SqlQuery<long>($"SELECT NEXT VALUE FOR dbo.PatientIdSequence AS [Value]")
            .SingleAsync(cancellationToken);

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        builder.Entity<Patient>().HasQueryFilter(p => !p.IsDeleted);
        builder.Entity<Appointment>().HasQueryFilter(a => !a.IsDeleted);
    }
}
