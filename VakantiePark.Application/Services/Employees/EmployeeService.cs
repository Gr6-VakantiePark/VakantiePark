using VakantiePark.Application.Extensions;
using VakantiePark.Application.Interfaces;
using VakantiePark.Domain.Entities;
using VakantiePark.Domain.Interfaces;
using VakantiePark.Shared.DTOs.Auth;

namespace VakantiePark.Application.Services
{
    public class EmployeeService(IEmployeeRepository repository, IRepository<Employee> employees, IPasswordHasher passwordHasher) : IEmployeeService
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

            var permissions = await FindPermissionsAsync(request.Permissions);

            var employee = request.ToEntity(email, passwordHasher.Hash(request.Password), permissions);

            employees.Add(employee);
            await employees.SaveChangesAsync();

            return employee.ToDto();
        }

        public async Task<EmployeeDto> UpdateAsync(Guid id, UpdateEmployeeDto request)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Het id van de medewerker ontbreekt.");

            request.Validate();

            var employee = await repository.GetForUpdateAsync(id)
                ?? throw new KeyNotFoundException("Medewerker is niet gevonden.");

            string email = EmployeeMappings.NormalizeEmail(request.Email);

            if (await repository.EmailExistsAsync(email, id))
                throw new ArgumentException("Er is al een medewerker met dit e-mailadres.");

            employee.Email = email;
            employee.FirstName = request.FirstName.Trim();
            employee.LastName = request.LastName.Trim();

            await employees.SaveChangesAsync();

            return employee.ToDto();
        }

        public async Task<EmployeeDto> SetActiveAsync(Guid id, SetEmployeeActiveDto request, Guid currentEmployeeId)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Het id van de medewerker ontbreekt.");
            if (request is null)
                throw new ArgumentException("De status van de medewerker ontbreekt.");
            if (!request.IsActive && id == currentEmployeeId)
                throw new ArgumentException("Je kunt je eigen account niet uitschakelen.");

            var employee = await repository.GetForUpdateAsync(id)
                ?? throw new KeyNotFoundException("Medewerker is niet gevonden.");

            if (employee.IsActive != request.IsActive)
            {
                employee.IsActive = request.IsActive;
                await employees.SaveChangesAsync();
            }

            return employee.ToDto();
        }

        public async Task<List<PermissionDto>> GetPermissionsAsync()
        {
            var permissions = await repository.GetAllPermissionsAsync();
            return permissions.Select(p => p.ToDto()).ToList();
        }

        public async Task<EmployeeDto> UpdatePermissionsAsync(Guid id, UpdateEmployeePermissionsDto request, Guid currentEmployeeId)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Het id van de medewerker ontbreekt.");

            request.Validate();

            if (id == currentEmployeeId && !request.IsOwner)
                throw new ArgumentException("Je kunt je eigen eigenaarsrechten niet afnemen.");

            var employee = await repository.GetForUpdateAsync(id)
                ?? throw new KeyNotFoundException("Medewerker is niet gevonden.");

            var permissions = await FindPermissionsAsync(request.Permissions);

            employee.IsOwner = request.IsOwner;
            employee.Permissions.Clear();
            foreach (var permission in permissions)
                employee.Permissions.Add(permission);

            await employees.SaveChangesAsync();

            return employee.ToDto();
        }

        private async Task<List<Permission>> FindPermissionsAsync(List<string> codes)
        {
            var requested = codes.Select(c => c.Trim().ToLowerInvariant()).Distinct().ToList();
            if (requested.Count == 0)
                return [];

            var permissions = await repository.GetPermissionsAsync(requested);
            var unknown = requested.Except(permissions.Select(p => p.Code), StringComparer.OrdinalIgnoreCase).ToList();
            if (unknown.Count > 0)
                throw new ArgumentException($"Onbekend recht: {string.Join(", ", unknown)}.");

            return permissions;
        }
    }
}
