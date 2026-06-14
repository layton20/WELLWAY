using MediatR;
using Wellway.Application.Common;
using Wellway.Application.Features.Patients.DTOs;

namespace Wellway.Application.Features.Patients.Queries.GetPatientById;

public sealed record GetPatientByIdQuery(Guid PatientId) : IRequest<Result<PatientDto>>;
