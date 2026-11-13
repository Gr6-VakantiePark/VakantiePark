using VakantiePark.Domain.Entities;

namespace VakantiePark.Domain.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<Employee?> GetByEmailAsync(string normalizedEmail);

        Task<Employee?> GetByIdAsync(Guid id);

        Task<Employee?> GetForUpdateAsync(Guid id);

        Task<List<Employee>> GetAllAsync();

        Task<bool> IsActiveAsync(Guid id);

        Task<bool> EmailExistsAsync(string normalizedEmail);

        Task<bool> EmailExistsAsync(string normalizedEmail, Guid excludedEmployeeId);

        Task<List<Permission>> GetPermissionsAsync(IEnumerable<string> codes);

        Task<List<Permission>> GetAllPermissionsAsync();
    }
}
