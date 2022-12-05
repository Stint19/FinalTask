using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FinalTask.NET6._0.Services.Interfaces
{
    public interface ITokenService
    {
        JwtSecurityToken CreateToken(List<Claim> authClaims);
        string GenerateRefreshToken();
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token);
    }
}
