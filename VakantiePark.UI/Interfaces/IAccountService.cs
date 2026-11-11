using VakantiePark.Shared.Common;
using VakantiePark.Shared.DTOs.Auth;

namespace VakantiePark.UI.Interfaces
{
    public interface IAccountService
    {
        Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginRequestDto request);

        Task LogoutAsync();

        Task<ApiResponse<EmployeeDto>> GetCurrentEmployeeAsync();

        Task<ApiResponse<List<EmployeeDto>>> GetEmployeesAsync();
    }
}
