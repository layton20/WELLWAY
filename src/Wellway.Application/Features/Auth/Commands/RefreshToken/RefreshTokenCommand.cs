using MediatR;
using Wellway.Application.Common;
using Wellway.Application.Features.Auth.DTOs;

namespace Wellway.Application.Features.Auth.Commands.RefreshToken;

public sealed record RefreshTokenCommand(string AccessToken, string RefreshToken)
    : IRequest<Result<AuthResponse>>;
