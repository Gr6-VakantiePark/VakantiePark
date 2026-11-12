using VakantiePark.Shared.DTOs.Auth;

namespace VakantiePark.Application.Interfaces
{
    public interface IEmployeeService
    {
        Task<List<EmployeeDto>> GetAllAsync();

        Task<EmployeeDto> GetByIdAsync(Guid id);

        Task<EmployeeDto> CreateAsync(CreateEmployeeDto request);

        Task<EmployeeDto> UpdateAsync(Guid id, UpdateEmployeeDto request);

        Task<EmployeeDto> SetActiveAsync(Guid id, SetEmployeeActiveDto request, Guid currentEmployeeId);
    }
}
