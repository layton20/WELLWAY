using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Wellway.Application.Common;
using Wellway.Application.Features.Auth.DTOs;
using Wellway.Application.Identity;
using Wellway.Application.Interfaces;

namespace Wellway.Application.Features.Auth.Commands.RefreshToken;

public sealed class RefreshTokenCommandHandler(
    UserManager<WellwayIdentityUser> userManager,
    ITokenService tokenService)
    : IRequestHandler<RefreshTokenCommand, Result<AuthResponse>>
{
    public async Task<Result<AuthResponse>> Handle(
        RefreshTokenCommand request,
        CancellationToken cancellationToken)
    {
        var principal = tokenService.GetPrincipalFromExpiredToken(request.AccessToken);
        if (principal is null)
            return Result<AuthResponse>.Failure(Error.Unauthorised("Invalid token."));

        var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
            return Result<AuthResponse>.Failure(Error.Unauthorised("Invalid token."));

        var user = await userManager.FindByIdAsync(userId);
        if (user is null
            || user.RefreshToken != request.RefreshToken
            || user.RefreshTokenExpiry <= DateTime.UtcNow)
            return Result<AuthResponse>.Failure(Error.Unauthorised("Invalid or expired refresh token."));

        var roles = await userManager.GetRolesAsync(user);
        var newAccessToken = tokenService.GenerateAccessToken(user, roles);
        var newRefreshToken = tokenService.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        await userManager.UpdateAsync(user);

        return Result<AuthResponse>.Success(new AuthResponse(
            AccessToken: newAccessToken,
            RefreshToken: newRefreshToken,
            AccessTokenExpiry: DateTime.UtcNow.AddMinutes(60),
            User: new UserProfileDto(
                Id: user.Id,
                Email: user.Email!,
                FullName: user.FullName,
                FirstName: user.FirstName,
                LastName: user.LastName,
                Roles: roles,
                StaffRole: user.StaffRole)));
    }
}
