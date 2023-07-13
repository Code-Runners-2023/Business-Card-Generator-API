using BusinessCardGenerator.API.Data;
using BusinessCardGenerator.API.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BusinessCardGenerator.API.Services
{
    public class TokenService : ITokenService
    {
        private readonly ApplicationSettings settings;

        public TokenService(IConfiguration config)
        {
            settings = new ApplicationSettings(config);
        }

        public string GenerateNewToken(User user)
        {
            var expiration = DateTime.UtcNow.AddMinutes(settings.JwtExpirationMinutes);
            
            var token = CreateJwtToken(
                CreateClaims(user),
                CreateSigningCredentials(),
                expiration
            );
            
            var tokenHandler = new JwtSecurityTokenHandler();
            
            return tokenHandler.WriteToken(token);
        }

        public JwtSecurityToken CreateJwtToken(List<Claim> claims, SigningCredentials credentials, DateTime expiration)
            => new(
                settings.JwtIssuer,
                settings.JwtAudience,
                claims,
                expires: expiration,
                signingCredentials: credentials
            );

        public List<Claim> CreateClaims(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, $"{user.FirstName} {user.LastName}")
            };

            return claims;
        }

        public SigningCredentials CreateSigningCredentials()
        {
            return new SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(settings.JwtSecretKey)
                ),
                SecurityAlgorithms.HmacSha256
            );
        }
    }
}
