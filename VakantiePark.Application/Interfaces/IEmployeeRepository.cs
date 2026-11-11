using VakantiePark.Domain.Entities;

namespace VakantiePark.Application.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<Employee?> GetByEmailAsync(string normalizedEmail);

        Task<Employee?> GetByIdAsync(Guid id);

        Task<List<Employee>> GetAllAsync();

        Task<bool> IsActiveAsync(Guid id);

        Task<bool> AnyAsync();

        Task<bool> EmailExistsAsync(string normalizedEmail);

        Task<List<Role>> GetRolesAsync(IEnumerable<string> codes);

        Task AddAsync(Employee employee);

        Task SaveChangesAsync();
    }
}
