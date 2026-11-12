using VakantiePark.Application.Extensions;
using VakantiePark.Application.Interfaces;
using VakantiePark.Domain.Entities;
using VakantiePark.Domain.Interfaces;
using VakantiePark.Shared.Common;
using VakantiePark.Shared.DTOs.Auth;

namespace VakantiePark.Application.Services
{
    public class EmployeeService(IEmployeeRepository repository, IPasswordHasher passwordHasher) : IEmployeeService
    {
        public async Task<List<EmployeeDto>> GetAllAsync()
        {
            var employees = await repository.GetAllAsync();
            return employees.Select(e => e.ToDto()).ToList();
        }

        public async Task<EmployeeDto> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Het id van de medewerker ontbreekt.");

            var employee = await repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("Medewerker is niet gevonden.");

            return employee.ToDto();
        }

        public async Task<EmployeeDto> CreateAsync(CreateEmployeeDto request)
        {
            request.Validate();

            string email = EmployeeMappings.NormalizeEmail(request.Email);

            if (await repository.EmailExistsAsync(email))
                throw new ArgumentException("Er is al een medewerker met dit e-mailadres.");

            var roles = await repository.GetRolesAsync(request.Roles);
            var unknown = request.Roles.Select(r => r.Trim()).Except(roles.Select(r => r.Code), StringComparer.OrdinalIgnoreCase).ToList();
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
    }
}
