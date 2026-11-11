using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VakantiePark.Application.Interfaces;
using VakantiePark.Shared.Common;
using VakantiePark.Shared.DTOs.Auth;

namespace VakantiePark.API.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController(IAuthService authService, IEmployeeService employeeService) : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType<ApiResponse<LoginResponseDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse<LoginResponseDto>>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ApiResponse<LoginResponseDto>>(StatusCodes.Status401Unauthorized)]
        public Task<IActionResult> Login(LoginRequestDto request) =>
            this.RunAsync(() => authService.LoginAsync(request));

        [Authorize]
        [HttpGet("me")]
        [ProducesResponseType<ApiResponse<EmployeeDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse<EmployeeDto>>(StatusCodes.Status401Unauthorized)]
        public Task<IActionResult> Me() =>
            this.RunAsEmployeeAsync(employeeService.GetByIdAsync);
    }
}
