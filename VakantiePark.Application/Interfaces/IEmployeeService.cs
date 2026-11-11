using VakantiePark.Shared.DTOs.Auth;

namespace VakantiePark.Application.Interfaces
{
    public interface IEmployeeService
    {
        Task<List<EmployeeDto>> GetAllAsync();

        Task<EmployeeDto> GetByIdAsync(Guid id);

        Task<EmployeeDto> CreateAsync(CreateEmployeeDto request);

        Task EnsureInitialAdministratorAsync(string email, string password);

        Task EnsureTestAccountsAsync(IEnumerable<CreateEmployeeDto> accounts);
    }
}
