using BusinessCardGenerator.API.Data;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BusinessCardGenerator.API.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateNewToken(User user);

        JwtSecurityToken CreateJwtToken(List<Claim> claims, SigningCredentials credentials,
            DateTime expiration);

        List<Claim> CreateClaims(User user);

        SigningCredentials CreateSigningCredentials();
    }
}
