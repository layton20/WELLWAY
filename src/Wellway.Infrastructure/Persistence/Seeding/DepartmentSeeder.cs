using Microsoft.EntityFrameworkCore;
using Wellway.Domain.Entities;
using Wellway.Infrastructure.Persistence;

namespace Wellway.Infrastructure.Persistence.Seeding;

public static class DepartmentSeeder
{
    private static readonly (string Name, string Description, string Extension)[] Data =
    [
        ("Accident & Emergency", "Provides immediate treatment for life-threatening conditions and major emergencies.", "2301"),
        ("Cardiology", "Specialises in the diagnosis and treatment of heart disease and cardiovascular conditions.", "2415"),
        ("Orthopaedics", "Focuses on the treatment of musculoskeletal conditions including bones, joints and tendons.", "2523"),
        ("Neurology", "Diagnoses and treats disorders of the nervous system including the brain and spinal cord.", "2640"),
        ("Oncology", "Provides diagnosis, treatment and management of cancer through surgery, chemotherapy and radiotherapy.", "2712"),
        ("Paediatrics", "Specialises in the medical care of infants, children and adolescents up to the age of 16.", "2834"),
        ("General Medicine", "Provides assessment and management of a wide range of medical conditions and specialist referrals.", "2956"),
        ("Radiology", "Uses medical imaging techniques including X-ray, MRI, CT and ultrasound to diagnose conditions.", "3010"),
    ];

    public static async Task<List<Department>> SeedAsync(WellwayDbContext context)
    {
        if (await context.Departments.AnyAsync())
            return await context.Departments.ToListAsync();

        var departments = Data
            .Select(d => new Department(d.Name, d.Description, d.Extension))
            .ToList();

        context.Departments.AddRange(departments);
        await context.SaveChangesAsync();

        return departments;
    }
}
