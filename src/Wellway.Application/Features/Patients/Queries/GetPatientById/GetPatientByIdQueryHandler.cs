using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Wellway.Application.Common;
using Wellway.Application.Features.Patients.DTOs;
using Wellway.Application.Interfaces;

namespace Wellway.Application.Features.Patients.Queries.GetPatientById;

public sealed class GetPatientByIdQueryHandler(IWellwayDbContext context, IMapper mapper)
    : IRequestHandler<GetPatientByIdQuery, Result<PatientDto>>
{
    public async Task<Result<PatientDto>> Handle(GetPatientByIdQuery request, CancellationToken cancellationToken)
    {
        var patient = await context.Patients
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == request.PatientId, cancellationToken);

        if (patient is null)
            return Result<PatientDto>.Failure(
                Error.NotFound($"Patient '{request.PatientId}' was not found."));

        return Result<PatientDto>.Success(mapper.Map<PatientDto>(patient));
    }
}
