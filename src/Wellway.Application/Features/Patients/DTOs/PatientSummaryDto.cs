namespace Wellway.Application.Features.Patients.DTOs;

public sealed record PatientSummaryDto(
    Guid Id,
    string HospitalPatientId,
    string? NhsNumber,
    string FirstName,
    string LastName,
    DateOnly DateOfBirth,
    string PhoneNumber,
    string? Email,
    string Status);
