using Microsoft.EntityFrameworkCore;
using Wellway.Domain.Entities;

namespace Wellway.Application.Interfaces;

public interface IWellwayDbContext
{
    DbSet<Patient> Patients { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<long> GetNextPatientSequenceValueAsync(CancellationToken cancellationToken = default);
}
