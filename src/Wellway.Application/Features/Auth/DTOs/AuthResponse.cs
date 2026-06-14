namespace Wellway.Application.Features.Auth.DTOs;

public sealed record AuthResponse(
    string AccessToken,
    string RefreshToken,
    DateTime AccessTokenExpiry,
    UserProfileDto User);
