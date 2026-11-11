using System.Net.Mail;
using VakantiePark.Application.Extensions;
using VakantiePark.Application.Interfaces;
using VakantiePark.Domain.Entities;
using VakantiePark.Shared.Common;
using VakantiePark.Shared.DTOs.Auth;

namespace VakantiePark.Application.Services
{
    public class EmployeeService(IEmployeeRepository repository, IPasswordHasher passwordHasher) : IEmployeeService
    {
        public const int MinimumPasswordLength = 12;

        public async Task<List<EmployeeDto>> GetAllAsync()
        {
            var employees = await repository.GetAllAsync();
            return employees.Select(e => e.ToDto()).ToList();
        }

        public async Task<EmployeeDto> GetByIdAsync(Guid id)
        {
            var employee = await repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("Medewerker is niet gevonden.");

            return employee.ToDto();
        }

        public async Task<EmployeeDto> CreateAsync(CreateEmployeeDto request)
        {
            string email = EmployeeMappings.NormalizeEmail(request.Email ?? string.Empty);

            if (!IsValidEmail(email))
                throw new ArgumentException("Vul een geldig e-mailadres in.");

            if (string.IsNullOrWhiteSpace(request.FirstName) || string.IsNullOrWhiteSpace(request.LastName))
                throw new ArgumentException("Vul een voor- en achternaam in.");

            ValidatePassword(request.Password);

            if (request.Roles is null || request.Roles.Count == 0)
                throw new ArgumentException("Kies minimaal één rol.");

            if (await repository.EmailExistsAsync(email))
                throw new ArgumentException("Er is al een medewerker met dit e-mailadres.");

            var roles = await repository.GetRolesAsync(request.Roles);
            var unknown = request.Roles.Except(roles.Select(r => r.Code), StringComparer.OrdinalIgnoreCase).ToList();
            if (unknown.Count > 0)
                throw new ArgumentException($"Onbekende rol: {string.Join(", ", unknown)}.");

            var employee = new Employee
            {
                Id = Guid.NewGuid(),
                Email = email,
                PasswordHash = passwordHasher.Hash(request.Password),
                FirstName = request.FirstName.Trim(),
                LastName = request.LastName.Trim(),
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                Roles = roles,
            };

            await repository.AddAsync(employee);
            await repository.SaveChangesAsync();

            return employee.ToDto();
        }

        private static void ValidatePassword(string? password)
        {
            if (string.IsNullOrEmpty(password) || password.Length < MinimumPasswordLength)
                throw new ArgumentException($"Het wachtwoord moet minimaal {MinimumPasswordLength} tekens hebben.");
        }

        private static bool IsValidEmail(string email) =>
            MailAddress.TryCreate(email, out var address) && address.Address == email;
    }
}
