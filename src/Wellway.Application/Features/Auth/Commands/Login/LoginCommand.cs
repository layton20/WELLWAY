using MediatR;
using Wellway.Application.Common;
using Wellway.Application.Features.Auth.DTOs;

namespace Wellway.Application.Features.Auth.Commands.Login;

public sealed record LoginCommand(string Email, string Password)
    : IRequest<Result<AuthResponse>>;
