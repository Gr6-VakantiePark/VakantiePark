using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VakantiePark.Application.Interfaces;
using VakantiePark.Shared.Common;
using VakantiePark.Shared.DTOs.Auth;

namespace VakantiePark.API.Controllers
{
    [ApiController]
    [Route("admin/employees")]
    [Authorize(Policy = Permissions.ManageEmployees)]
    public class EmployeesController(IEmployeeService employeeService) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType<ApiResponse<List<EmployeeDto>>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status403Forbidden)]
        public Task<IActionResult> GetAll() =>
            this.RunAsync(employeeService.GetAllAsync);

        [HttpPost]
        [ProducesResponseType<ApiResponse<EmployeeDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse<EmployeeDto>>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status403Forbidden)]
        public Task<IActionResult> Create(CreateEmployeeDto request) =>
            this.RunAsync(() => employeeService.CreateAsync(request), _ => "Medewerker is aangemaakt.");
    }
}
