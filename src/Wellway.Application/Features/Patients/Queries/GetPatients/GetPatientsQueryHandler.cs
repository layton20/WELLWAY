using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Wellway.Application.Common;
using Wellway.Application.Features.Patients.DTOs;
using Wellway.Application.Interfaces;

namespace Wellway.Application.Features.Patients.Queries.GetPatients;

public sealed class GetPatientsQueryHandler(IWellwayDbContext context, IMapper mapper)
    : IRequestHandler<GetPatientsQuery, PaginatedResult<PatientSummaryDto>>
{
    public async Task<PaginatedResult<PatientSummaryDto>> Handle(
        GetPatientsQuery request,
        CancellationToken cancellationToken)
    {
        var query = context.Patients.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var term = request.SearchTerm.ToLower();
            query = query.Where(p =>
                p.FirstName.ToLower().Contains(term) ||
                p.LastName.ToLower().Contains(term) ||
                (p.NhsNumber != null && p.NhsNumber.Contains(term)) ||
                p.HospitalPatientId.ToLower().Contains(term));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var patients = await query
            .OrderBy(p => p.LastName)
            .ThenBy(p => p.FirstName)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        var dtos = mapper.Map<List<PatientSummaryDto>>(patients);
        return PaginatedResult<PatientSummaryDto>.Create(dtos, totalCount, request.PageNumber, request.PageSize);
    }
}
