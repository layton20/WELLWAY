using MediatR;
using Microsoft.AspNetCore.Identity;
using Wellway.Application.Common;
using Wellway.Application.Features.Auth.DTOs;
using Wellway.Application.Identity;
using Wellway.Application.Interfaces;

namespace Wellway.Application.Features.Auth.Commands.Login;

public sealed class LoginCommandHandler(
    UserManager<WellwayIdentityUser> userManager,
    ITokenService tokenService)
    : IRequestHandler<LoginCommand, Result<AuthResponse>>
{
    public async Task<Result<AuthResponse>> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
            return Result<AuthResponse>.Failure(Error.Unauthorised("Invalid email or password."));

        var passwordValid = await userManager.CheckPasswordAsync(user, request.Password);
        if (!passwordValid)
            return Result<AuthResponse>.Failure(Error.Unauthorised("Invalid email or password."));

        if (!user.IsActive)
            return Result<AuthResponse>.Failure(Error.Unauthorised("Account is inactive."));

        var roles = await userManager.GetRolesAsync(user);
        var accessToken = tokenService.GenerateAccessToken(user, roles);
        var refreshToken = tokenService.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        await userManager.UpdateAsync(user);

        return Result<AuthResponse>.Success(new AuthResponse(
            AccessToken: accessToken,
            RefreshToken: refreshToken,
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
