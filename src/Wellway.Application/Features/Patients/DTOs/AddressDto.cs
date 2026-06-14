namespace Wellway.Application.Features.Patients.DTOs;

public sealed record AddressDto(
    string Line1,
    string? Line2,
    string City,
    string? County,
    string PostCode,
    string? Country);
