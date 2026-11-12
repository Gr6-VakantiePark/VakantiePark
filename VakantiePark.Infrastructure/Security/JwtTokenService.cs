using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using VakantiePark.Application.Common;
using VakantiePark.Domain.Entities;
using VakantiePark.Domain.Interfaces;
using VakantiePark.Shared.Common;

namespace VakantiePark.Infrastructure.Security
{
    public class JwtTokenService(IOptions<JwtSettings> options) : ITokenService
    {
        private readonly JwtSettings _settings = options.Value;

        public (string Token, DateTime ExpiresAt) CreateToken(Employee employee)
        {
            ArgumentNullException.ThrowIfNull(employee);

            DateTime expiresAt = DateTime.UtcNow.AddMinutes(_settings.ExpiryMinutes);

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
            claims.AddRange(employee.Permissions.Select(permission => new Claim(Permissions.ClaimType, permission.Code)));

            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = _settings.Issuer,
                Audience = _settings.EffectiveAudience,
                Expires = expiresAt,
                SigningCredentials = new SigningCredentials(CreateSigningKey(_settings), SecurityAlgorithms.HmacSha256),
            };

            return (new JsonWebTokenHandler().CreateToken(descriptor), expiresAt);
        }

        public static SymmetricSecurityKey CreateSigningKey(JwtSettings settings)
        {
            ArgumentNullException.ThrowIfNull(settings);

            byte[] key = Encoding.UTF8.GetBytes(settings.Key ?? string.Empty);
            if (key.Length < JwtSettings.MinimumKeyLength)
                throw new InvalidOperationException(
                    "JWT:Key ontbreekt of is korter dan 32 tekens. Zet de sleutel in user-secrets "
                    + "(dotnet user-secrets set \"JWT:Key\" \"<sleutel>\" --project VakantiePark.API) "
                    + "of in de omgevingsvariabele JWT__Key.");

            return new SymmetricSecurityKey(key);
        }
    }
}
