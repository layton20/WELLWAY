using System.Security.Claims;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Wellway.Application.Common;
using Wellway.Application.Features.Patients.DTOs;
using Wellway.Application.Interfaces;
using Wellway.Domain.Entities;
using Wellway.Domain.Enums;
using Wellway.Domain.ValueObjects;

namespace Wellway.Application.Features.Patients.Commands.RegisterPatient;

public sealed class RegisterPatientCommandHandler(
    IWellwayDbContext context,
    IMapper mapper,
    IHttpContextAccessor httpContextAccessor)
    : IRequestHandler<RegisterPatientCommand, Result<PatientDto>>
{
    public async Task<Result<PatientDto>> Handle(RegisterPatientCommand request, CancellationToken cancellationToken)
    {
        if (request.NhsNumber is not null)
        {
            var nhsDuplicate = await context.Patients
                .AnyAsync(p => p.NhsNumber == request.NhsNumber, cancellationToken);
            if (nhsDuplicate)
                return Result<PatientDto>.Failure(
                    Error.Conflict("A patient with this NHS number already exists."));
        }

        if (!Enum.TryParse<BiologicalSex>(request.BiologicalSex, ignoreCase: true, out var biologicalSex))
            return Result<PatientDto>.Failure(Error.Validation("Invalid BiologicalSex value."));

        var bloodType = BloodType.Unknown;
        if (request.BloodType is not null)
        {
            if (!Enum.TryParse<BloodType>(request.BloodType, ignoreCase: true, out bloodType))
                return Result<PatientDto>.Failure(Error.Validation("Invalid BloodType value."));
        }

        var address = new Address(
            request.Address.Line1,
            request.Address.Line2,
            request.Address.City,
            request.Address.County,
            request.Address.PostCode,
            request.Address.Country);

        var createdBy = httpContextAccessor.HttpContext?
            .User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "unknown";

        var patient = new Patient(
            createdBy: createdBy,
            firstName: request.FirstName,
            lastName: request.LastName,
            dateOfBirth: request.DateOfBirth,
            biologicalSex: biologicalSex,
            address: address,
            phoneNumber: request.PhoneNumber,
            emergencyContactName: request.EmergencyContactName,
            emergencyContactPhone: request.EmergencyContactPhone,
            emergencyContactRelationship: request.EmergencyContactRelationship,
            nhsNumber: request.NhsNumber,
            preferredName: request.PreferredName,
            genderIdentity: request.GenderIdentity,
            pronouns: request.Pronouns,
            email: request.Email,
            bloodType: bloodType);

        context.Patients.Add(patient);
        await context.SaveChangesAsync(cancellationToken);

        return Result<PatientDto>.Success(mapper.Map<PatientDto>(patient));
    }
}
