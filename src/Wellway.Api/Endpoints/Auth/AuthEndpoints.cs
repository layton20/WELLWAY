using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Wellway.Application.Features.Auth.Commands.Login;
using Wellway.Application.Features.Auth.Commands.RefreshToken;
using Wellway.Application.Identity;

namespace Wellway.Api.Endpoints.Auth;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth").WithTags("Auth");

        group.MapPost("/login", async (LoginCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.Problem(
                    title: "Authentication failed",
                    detail: result.Error.Message,
                    statusCode: StatusCodes.Status401Unauthorized);
        });

        group.MapPost("/refresh", async (RefreshTokenCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.Problem(
                    title: "Token refresh failed",
                    detail: result.Error.Message,
                    statusCode: StatusCodes.Status401Unauthorized);
        });

        group.MapPost("/logout", async (
            ClaimsPrincipal principal,
            UserManager<WellwayIdentityUser> userManager) =>
        {
            var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null) return Results.Unauthorized();

            var user = await userManager.FindByIdAsync(userId);
            if (user is null) return Results.Unauthorized();

            user.RefreshToken = null;
            user.RefreshTokenExpiry = null;
            await userManager.UpdateAsync(user);

            return Results.NoContent();
        }).RequireAuthorization();

        return app;
    }
}
