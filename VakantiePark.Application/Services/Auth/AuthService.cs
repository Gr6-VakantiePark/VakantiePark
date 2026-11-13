using VakantiePark.Application.Extensions;
using VakantiePark.Application.Interfaces;
using VakantiePark.Domain.Entities;
using VakantiePark.Domain.Interfaces;
using VakantiePark.Shared.DTOs.Auth;

namespace VakantiePark.Application.Services
{
    public class AuthService(IEmployeeRepository repository, IRepository<Employee> employees, IPasswordHasher passwordHasher, ITokenService tokenService) : IAuthService
    {
        public const string InvalidCredentials = "Het e-mailadres of wachtwoord klopt niet.";
        public const string AccountDisabled = "Dit account is uitgeschakeld. Neem contact op met een beheerder.";

        private static string? _dummyHash;

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
        {
            request.Validate();

            var employee = await repository.GetByEmailAsync(EmployeeMappings.NormalizeEmail(request.Email));

            if (employee is null)
            {
                _dummyHash ??= passwordHasher.Hash(Guid.NewGuid().ToString());
                passwordHasher.Verify(request.Password, _dummyHash);
                throw new UnauthorizedAccessException(InvalidCredentials);
            }

            if (!passwordHasher.Verify(request.Password, employee.PasswordHash))
                throw new UnauthorizedAccessException(InvalidCredentials);

            if (!employee.IsActive)
                throw new UnauthorizedAccessException(AccountDisabled);

            employee.LastLoginAt = DateTime.UtcNow;
            await employees.SaveChangesAsync();

            var (token, expiresAt) = tokenService.CreateToken(employee);

            return employee.ToLoginResponse(token, expiresAt);
        }
    }
}
