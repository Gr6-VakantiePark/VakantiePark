using VakantiePark.Domain.Entities;
using VakantiePark.Shared.DTOs.Auth;

namespace VakantiePark.Application.Extensions
{
    public static class EmployeeMappings
    {
        public static EmployeeDto ToDto(this Employee employee) => new()
        {
            Id = employee.Id,
            Email = employee.Email,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            IsActive = employee.IsActive,
            LastLoginAt = employee.LastLoginAt,
            Roles = employee.Roles.Select(r => r.Code).Order().ToList(),
            RoleNames = employee.Roles.Select(r => r.Name).Order().ToList(),
            Permissions = employee.Permissions.Select(p => p.Code).Order().ToList(),
            PermissionNames = employee.Permissions.Select(p => p.Name).Order().ToList(),
        };

        public static string NormalizeEmail(string email) => email.Trim().ToLowerInvariant();
    }
}
