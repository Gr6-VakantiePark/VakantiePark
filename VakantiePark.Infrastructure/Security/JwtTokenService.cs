using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using VakantiePark.Application.Common;
using VakantiePark.Application.Interfaces;
using VakantiePark.Domain.Entities;

namespace VakantiePark.Infrastructure.Security
{
    public class JwtTokenService(JwtSettings settings) : ITokenService
    {
        public const string PermissionClaim = "permission";

        public (string Token, DateTime ExpiresAt) CreateToken(Employee employee)
        {
            DateTime expiresAt = DateTime.UtcNow.AddMinutes(settings.ExpiryMinutes);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, employee.Id.ToString()),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Email, employee.Email),
                new(JwtRegisteredClaimNames.Name, $"{employee.FirstName} {employee.LastName}"),
                new(JwtRegisteredClaimNames.GivenName, employee.FirstName),
                new(JwtRegisteredClaimNames.FamilyName, employee.LastName),
            };
            claims.AddRange(employee.Roles.Select(role => new Claim("role", role.Code)));
            claims.AddRange(employee.Permissions.Select(permission => new Claim(PermissionClaim, permission.Code)));

            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = settings.Issuer,
                Audience = settings.EffectiveAudience,
                Expires = expiresAt,
                SigningCredentials = new SigningCredentials(CreateSigningKey(settings), SecurityAlgorithms.HmacSha256),
            };

            return (new JsonWebTokenHandler().CreateToken(descriptor), expiresAt);
        }

        public static SymmetricSecurityKey CreateSigningKey(JwtSettings settings)
        {
            byte[] key = Encoding.UTF8.GetBytes(settings.Key);
            if (key.Length < 32)
                throw new InvalidOperationException("JWT:Key moet minimaal 32 tekens lang zijn.");

            return new SymmetricSecurityKey(key);
        }
    }
}
