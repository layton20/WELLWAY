using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Wellway.Application.Identity;
using Wellway.Domain.Enums;

namespace Wellway.Infrastructure.Identity;

public static class UserSeeder
{
    private static readonly string[] RoleNames = ["Administrator", "Doctor", "Receptionist", "Nurse"];

    private static readonly (string Email, string Password, string FirstName, string LastName, StaffRole? StaffRole, string Role)[] Users =
    [
        ("admin@wellway.nhs.uk",        "Admin1234!",     "System", "Administrator", null,                   "Administrator"),
        ("doctor@wellway.nhs.uk",       "Doctor1234!",    "James",  "Webb",          StaffRole.Doctor,        "Doctor"),
        ("receptionist@wellway.nhs.uk", "Reception1234!", "Sarah",  "Mitchell",      StaffRole.Receptionist,  "Receptionist"),
        ("nurse@wellway.nhs.uk",        "Nurse1234!",     "Emily",  "Clarke",        StaffRole.Nurse,         "Nurse"),
    ];

    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<WellwayIdentityUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        if (await userManager.Users.AnyAsync()) return;

        foreach (var roleName in RoleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
                await roleManager.CreateAsync(new IdentityRole(roleName));
        }

        foreach (var (email, password, firstName, lastName, staffRole, role) in Users)
        {
            var user = new WellwayIdentityUser
            {
                UserName = email,
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                StaffRole = staffRole,
                IsActive = true,
                EmailConfirmed = true,
            };

            var result = await userManager.CreateAsync(user, password);
            if (result.Succeeded)
                await userManager.AddToRoleAsync(user, role);
        }
    }
}
