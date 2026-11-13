using Microsoft.EntityFrameworkCore;
using VakantiePark.Domain.Entities;
using VakantiePark.Domain.Interfaces;
using VakantiePark.Infrastructure.Data;

namespace VakantiePark.Infrastructure.Repositories
{
    public class EmployeeRepository(ApplicationDBContext context) : IEmployeeRepository
    {
        private IQueryable<Employee> WithPermissions =>
            context.Employees.Include(e => e.Permissions);

        public Task<Employee?> GetByEmailAsync(string normalizedEmail) =>
            WithPermissions.FirstOrDefaultAsync(e => e.Email == normalizedEmail);

        public Task<Employee?> GetByIdAsync(Guid id) =>
            WithPermissions.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);

        public Task<Employee?> GetForUpdateAsync(Guid id) =>
            WithPermissions.FirstOrDefaultAsync(e => e.Id == id);

        public Task<List<Employee>> GetAllAsync() =>
            WithPermissions.AsNoTracking()
                .OrderBy(e => e.LastName)
                .ThenBy(e => e.FirstName)
                .ToListAsync();

        public Task<bool> EmailExistsAsync(string normalizedEmail) =>
            context.Employees.AnyAsync(e => e.Email == normalizedEmail);

        public Task<bool> EmailExistsAsync(string normalizedEmail, Guid excludedEmployeeId) =>
            context.Employees.AnyAsync(e => e.Email == normalizedEmail && e.Id != excludedEmployeeId);

        public Task<List<Permission>> GetPermissionsAsync(IEnumerable<string> codes)
        {
            ArgumentNullException.ThrowIfNull(codes);

            var normalized = codes.Select(c => c.Trim().ToLowerInvariant()).Distinct().ToList();
            return context.Permissions
                .Where(p => normalized.Contains(p.Code))
                .ToListAsync();
        }

        public Task<List<Permission>> GetAllPermissionsAsync() =>
            context.Permissions.AsNoTracking().OrderBy(p => p.Name).ToListAsync();
    }
}
