using MediatR;
using Wellway.Application.Common;
using Wellway.Application.Features.Patients.DTOs;

namespace Wellway.Application.Features.Patients.Queries.GetPatients;

public sealed record GetPatientsQuery(
    string? SearchTerm,
    int PageNumber = 1,
    int PageSize = 20) : IRequest<PaginatedResult<PatientSummaryDto>>;
