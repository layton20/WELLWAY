using Microsoft.AspNetCore.Identity;
using Wellway.Domain.Enums;

namespace Wellway.Application.Identity;

public sealed class WellwayIdentityUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public StaffRole? StaffRole { get; set; }
    public bool IsActive { get; set; } = true;
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiry { get; set; }

    public string FullName => $"{FirstName} {LastName}";
}
