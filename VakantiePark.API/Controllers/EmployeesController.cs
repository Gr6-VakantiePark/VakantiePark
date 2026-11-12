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

        [HttpPut("{id:guid}")]
        [ProducesResponseType<ApiResponse<EmployeeDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse<EmployeeDto>>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status403Forbidden)]
        [ProducesResponseType<ApiResponse<EmployeeDto>>(StatusCodes.Status404NotFound)]
        public Task<IActionResult> Update(Guid id, UpdateEmployeeDto request) =>
            this.RunAsync(() => employeeService.UpdateAsync(id, request), _ => "Medewerker is gewijzigd.");

        [HttpPut("{id:guid}/active")]
        [ProducesResponseType<ApiResponse<EmployeeDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse<EmployeeDto>>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status403Forbidden)]
        [ProducesResponseType<ApiResponse<EmployeeDto>>(StatusCodes.Status404NotFound)]
        public Task<IActionResult> SetActive(Guid id, SetEmployeeActiveDto request) =>
            this.RunAsEmployeeAsync(
                currentEmployeeId => employeeService.SetActiveAsync(id, request, currentEmployeeId),
                employee => employee.IsActive ? "Medewerker is ingeschakeld." : "Medewerker is uitgeschakeld.");
    }
}
