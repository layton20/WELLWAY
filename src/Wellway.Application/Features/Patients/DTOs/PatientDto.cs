namespace Wellway.Application.Features.Patients.DTOs;

public sealed record PatientDto(
    Guid Id,
    string HospitalPatientId,
    string? NhsNumber,
    string FirstName,
    string LastName,
    string? PreferredName,
    DateOnly DateOfBirth,
    string BiologicalSex,
    string? GenderIdentity,
    string? Pronouns,
    AddressDto Address,
    string PhoneNumber,
    string? Email,
    string EmergencyContactName,
    string EmergencyContactPhone,
    string EmergencyContactRelationship,
    string BloodType,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);
