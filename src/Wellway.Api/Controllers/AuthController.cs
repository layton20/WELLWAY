using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Wellway.Application.Features.Auth.Commands.Login;
using Wellway.Application.Features.Auth.Commands.RefreshToken;
using Wellway.Application.Identity;

namespace Wellway.Api.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController( 
    ISender sender,
    UserManager<WellwayIdentityUser> userManager) : ControllerBase
{
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginCommand command, CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return result.IsSuccess
            ? Ok(result.Value)
            : Problem(title: "Authentication failed", detail: result.Error.Message, statusCode: StatusCodes.Status401Unauthorized);
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<IActionResult> Refresh(RefreshTokenCommand command, CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return result.IsSuccess
            ? Ok(result.Value)
            : Problem(title: "Token refresh failed", detail: result.Error.Message, statusCode: StatusCodes.Status401Unauthorized);
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null) return Unauthorized();

        var user = await userManager.FindByIdAsync(userId);
        if (user is null) return Unauthorized();

        user.RefreshToken = null;
        user.RefreshTokenExpiry = null;
        await userManager.UpdateAsync(user);

        return NoContent();
    }
}
