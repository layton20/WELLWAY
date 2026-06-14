using MediatR;
using Wellway.Application.Common;
using Wellway.Application.Features.Patients.DTOs;

namespace Wellway.Application.Features.Patients.Commands.RegisterPatient;

public sealed record RegisterPatientCommand(
    string FirstName,
    string LastName,
    string? PreferredName,
    DateOnly DateOfBirth,
    string BiologicalSex,
    string? GenderIdentity,
    string? Pronouns,
    string? NhsNumber,
    AddressDto Address,
    string PhoneNumber,
    string? Email,
    string EmergencyContactName,
    string EmergencyContactPhone,
    string EmergencyContactRelationship,
    string? BloodType) : IRequest<Result<PatientDto>>;
