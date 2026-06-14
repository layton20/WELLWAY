using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wellway.Application.Common;
using Wellway.Application.Features.Patients.Commands.RegisterPatient;
using Wellway.Application.Features.Patients.Queries.GetPatientById;
using Wellway.Application.Features.Patients.Queries.GetPatients;

namespace Wellway.Api.Controllers;

[ApiController]
[Route("api/patients")]
[Authorize]
public sealed class PatientController(ISender sender) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> RegisterPatient(
        RegisterPatientCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        if (result.IsFailure)
            return ToHttpResponse(result.Error);

        return CreatedAtAction(
            nameof(GetPatient),
            new { patientId = result.Value!.Id },
            result.Value);
    }

    [HttpGet("{patientId:guid}")]
    public async Task<IActionResult> GetPatient(Guid patientId, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetPatientByIdQuery(patientId), cancellationToken);
        if (result.IsFailure)
            return ToHttpResponse(result.Error);

        return Ok(result.Value);
    }

    [HttpGet]
    public async Task<IActionResult> GetPatients(
        [FromQuery] string? searchTerm,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await sender.Send(
            new GetPatientsQuery(searchTerm, pageNumber, pageSize),
            cancellationToken);
        return Ok(result);
    }

    private IActionResult ToHttpResponse(Error error) => error.Code switch
    {
        "NotFound" => Problem(detail: error.Message, statusCode: StatusCodes.Status404NotFound),
        "Conflict" => Problem(detail: error.Message, statusCode: StatusCodes.Status409Conflict),
        "Validation" => Problem(detail: error.Message, statusCode: StatusCodes.Status400BadRequest),
        "Unauthorised" => Problem(detail: error.Message, statusCode: StatusCodes.Status401Unauthorized),
        _ => Problem(detail: error.Message, statusCode: StatusCodes.Status500InternalServerError),
    };
}
