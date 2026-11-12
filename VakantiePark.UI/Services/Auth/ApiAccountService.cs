using VakantiePark.Shared.Common;
using VakantiePark.Shared.DTOs.Auth;
using VakantiePark.UI.Authentication;
using VakantiePark.UI.Interfaces;

namespace VakantiePark.UI.Services
{
    public class ApiAccountService(IHttpService http, AuthStateProvider authStateProvider) : IAccountService
    {
        public async Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginRequestDto request)
        {
            var response = await http.PostAsync<LoginResponseDto, LoginRequestDto>("auth/login", request);

            if (response.Succeeded && response.Value is not null)
                await authStateProvider.MarkUserAsAuthenticatedAsync(response.Value.Token);

            return response;
        }

        public Task LogoutAsync() => authStateProvider.MarkUserAsLoggedOutAsync();

        public Task<ApiResponse<EmployeeDto>> GetCurrentEmployeeAsync() =>
            http.GetAsync<EmployeeDto>("auth/me");

        public Task<ApiResponse<List<EmployeeDto>>> GetEmployeesAsync() =>
            http.GetAsync<List<EmployeeDto>>("admin/employees");

        public Task<ApiResponse<EmployeeDto>> UpdateEmployeeAsync(Guid id, UpdateEmployeeDto request) =>
            http.PutAsync<EmployeeDto, UpdateEmployeeDto>($"admin/employees/{id}", request);

        public Task<ApiResponse<EmployeeDto>> SetEmployeeActiveAsync(Guid id, bool isActive) =>
            http.PutAsync<EmployeeDto, SetEmployeeActiveDto>($"admin/employees/{id}/active", new SetEmployeeActiveDto { IsActive = isActive });
    }
}
