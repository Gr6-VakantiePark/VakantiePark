using Microsoft.EntityFrameworkCore;
using VakantiePark.Application.Interfaces;
using VakantiePark.Domain.Entities;
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

        public Task<List<Employee>> GetAllAsync() =>
            WithRoles.AsNoTracking()
                .OrderBy(e => e.LastName)
                .ThenBy(e => e.FirstName)
                .ToListAsync();

        public Task<bool> IsActiveAsync(Guid id) =>
            context.Employees.AnyAsync(e => e.Id == id && e.IsActive);

        public Task<bool> AnyAsync() => context.Employees.AnyAsync();

        public Task<bool> EmailExistsAsync(string normalizedEmail) =>
            context.Employees.AnyAsync(e => e.Email == normalizedEmail);

        public Task<List<Role>> GetRolesAsync(IEnumerable<string> codes)
        {
            ArgumentNullException.ThrowIfNull(codes);

            var normalized = codes.Select(c => c.Trim().ToLowerInvariant()).ToList();
            return context.Roles
                .Include(r => r.Permissions)
                .Where(r => normalized.Contains(r.Code))
                .ToListAsync();
        }

        public async Task AddAsync(Employee employee)
        {
            ArgumentNullException.ThrowIfNull(employee);
            await context.Employees.AddAsync(employee);
        }

        public Task SaveChangesAsync() => context.SaveChangesAsync();
    }
}
