using Microsoft.EntityFrameworkCore;
using VakantiePark.Domain.Entities;
using VakantiePark.Domain.Interfaces;
using VakantiePark.Infrastructure.Data;

namespace VakantiePark.Infrastructure.Repositories
{
    public class EmployeeRepository(ApplicationDBContext context) : IEmployeeRepository
    {
        private IQueryable<Employee> WithRoles =>
            context.Employees.Include(e => e.Roles).ThenInclude(r => r.Permissions);

        public Task<Employee?> GetByEmailAsync(string normalizedEmail) =>
            WithRoles.FirstOrDefaultAsync(e => e.Email == normalizedEmail);

        public Task<Employee?> GetByIdAsync(Guid id) =>
            WithRoles.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);

        public Task<Employee?> GetForUpdateAsync(Guid id) =>
            WithRoles.FirstOrDefaultAsync(e => e.Id == id);

        public Task<List<Employee>> GetAllAsync() =>
            WithRoles.AsNoTracking()
                .OrderBy(e => e.LastName)
                .ThenBy(e => e.FirstName)
                .ToListAsync();

        public Task<bool> IsActiveAsync(Guid id) =>
            context.Employees.AnyAsync(e => e.Id == id && e.IsActive);

        public Task<bool> EmailExistsAsync(string normalizedEmail) =>
            context.Employees.AnyAsync(e => e.Email == normalizedEmail);

        public Task<bool> EmailExistsAsync(string normalizedEmail, Guid excludedEmployeeId) =>
            context.Employees.AnyAsync(e => e.Email == normalizedEmail && e.Id != excludedEmployeeId);

        public Task<List<Role>> GetRolesAsync(IEnumerable<string> codes)
        {
            ArgumentNullException.ThrowIfNull(codes);

            var normalized = codes.Select(c => c.Trim().ToLowerInvariant()).ToList();
            return context.Roles
                .Include(r => r.Permissions)
                .Where(r => normalized.Contains(r.Code))
                .ToListAsync();
        }
    }
}
