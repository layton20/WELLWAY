using Wellway.Domain.Enums;

namespace Wellway.Application.Features.Auth.DTOs;

public sealed record UserProfileDto(
    string Id,
    string Email,
    string FullName,
    string FirstName,
    string LastName,
    IList<string> Roles,
    StaffRole? StaffRole);
