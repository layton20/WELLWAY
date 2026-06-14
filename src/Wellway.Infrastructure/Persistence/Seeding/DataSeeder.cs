using Bogus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Wellway.Infrastructure.Persistence;

namespace Wellway.Infrastructure.Persistence.Seeding;

public static class DataSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<WellwayDbContext>();
        var logger = scope.ServiceProvider
            .GetRequiredService<ILoggerFactory>()
            .CreateLogger("Wellway.DataSeeder");

        try
        {
            // Fixed seed ensures reproducible data across runs
            Randomizer.Seed = new Random(20260101);

            logger.LogInformation("Seeding departments...");
            var departments = await DepartmentSeeder.SeedAsync(context);

            logger.LogInformation("Seeding staff...");
            var staff = await StaffSeeder.SeedAsync(context, departments);

            logger.LogInformation("Seeding patients...");
            var patients = await PatientSeeder.SeedAsync(context);

            logger.LogInformation("Seeding appointments...");
            await AppointmentSeeder.SeedAsync(context, patients, staff, departments);

            logger.LogInformation(
                "Seeding complete — Departments: {D}, Staff: {S}, Patients: {P}",
                departments.Count, staff.Count, patients.Count);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database");
        }
    }
}
