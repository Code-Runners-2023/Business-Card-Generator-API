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
        private readonly string issuer;
        private readonly string audience;
        private readonly string secretKey;
        private readonly double expirationMinutes;

        public TokenService(IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings") ??
                              throw new InvalidOperationException("'JwtSettings' not found.");

            issuer = jwtSettings["Issuer"] ?? throw new InvalidOperationException("JWT Issuer not found!");
            
            audience = jwtSettings["Audience"] ?? throw new InvalidOperationException("JWT Audience not found!");
            
            secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey not found!");
            
            expirationMinutes = double.Parse(jwtSettings["ExpirationMinutes"] ??
                                throw new InvalidOperationException("JWT ExpirationMinutes not found!"));
        }

        public string GenerateNewToken(User user)
        {
            var expiration = DateTime.UtcNow.AddMinutes(expirationMinutes);
            
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
                issuer,
                audience,
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
                    Encoding.UTF8.GetBytes(secretKey)
                ),
                SecurityAlgorithms.HmacSha256
            );
        }
    }
}
