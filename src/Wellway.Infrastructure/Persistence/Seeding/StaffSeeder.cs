using Bogus;
using Microsoft.EntityFrameworkCore;
using Wellway.Domain.Entities;
using Wellway.Domain.Enums;
using Wellway.Infrastructure.Persistence;

namespace Wellway.Infrastructure.Persistence.Seeding;

public static class StaffSeeder
{
    private static readonly Dictionary<string, (string DoctorTitle, StaffRole SecondRole, string SecondTitle)> Config = new()
    {
        ["Accident & Emergency"] = ("Consultant Emergency Physician", StaffRole.Nurse, "Senior Emergency Nurse"),
        ["Cardiology"]           = ("Consultant Cardiologist",        StaffRole.Receptionist, "Ward Receptionist"),
        ["Orthopaedics"]         = ("Consultant Orthopaedic Surgeon", StaffRole.Nurse, "Senior Orthopaedic Nurse"),
        ["Neurology"]            = ("Consultant Neurologist",         StaffRole.Receptionist, "Ward Receptionist"),
        ["Oncology"]             = ("Consultant Oncologist",          StaffRole.Nurse, "Senior Oncology Nurse"),
        ["Paediatrics"]          = ("Consultant Paediatrician",       StaffRole.Receptionist, "Ward Receptionist"),
        ["General Medicine"]     = ("Consultant Physician",           StaffRole.Nurse, "Senior Clinical Nurse"),
        ["Radiology"]            = ("Consultant Radiologist",         StaffRole.Receptionist, "Ward Receptionist"),
    };

    public static async Task<List<Staff>> SeedAsync(WellwayDbContext context, List<Department> departments)
    {
        if (await context.StaffMembers.AnyAsync())
            return await context.StaffMembers.ToListAsync();

        var faker = new Faker("en_GB");
        var staffMembers = new List<Staff>();

        foreach (var department in departments)
        {
            if (!Config.TryGetValue(department.Name, out var cfg)) continue;

            staffMembers.Add(new Staff(
                userId: Guid.NewGuid().ToString(),
                firstName: faker.Name.FirstName(),
                lastName: faker.Name.LastName(),
                jobTitle: cfg.DoctorTitle,
                staffRole: StaffRole.Doctor,
                departmentId: department.Id,
                phoneExtension: faker.Random.ReplaceNumbers("####")));

            staffMembers.Add(new Staff(
                userId: Guid.NewGuid().ToString(),
                firstName: faker.Name.FirstName(),
                lastName: faker.Name.LastName(),
                jobTitle: cfg.SecondTitle,
                staffRole: cfg.SecondRole,
                departmentId: department.Id,
                phoneExtension: faker.Random.ReplaceNumbers("####")));
        }

        context.StaffMembers.AddRange(staffMembers);
        await context.SaveChangesAsync();

        return staffMembers;
    }
}
