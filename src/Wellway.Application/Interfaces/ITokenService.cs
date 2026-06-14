using System.Security.Claims;
using Wellway.Application.Identity;

namespace Wellway.Application.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(WellwayIdentityUser user, IList<string> roles);
    string GenerateRefreshToken();
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}
