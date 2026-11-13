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
            IsOwner = employee.IsOwner,
            LastLoginAt = employee.LastLoginAt,
            Permissions = employee.Permissions.Select(p => p.Code).Order().ToList(),
            PermissionNames = employee.Permissions.Select(p => p.Name).Order().ToList(),
        };

        public static LoginResponseDto ToLoginResponse(this Employee employee, string token, DateTime expiresAt) => new()
        {
            Token = token,
            ExpiresAt = expiresAt,
            Employee = employee.ToDto(),
        };

        public static PermissionDto ToDto(this Permission permission) => new()
        {
            Code = permission.Code,
            Name = permission.Name,
        };

        public static Employee ToEntity(this CreateEmployeeDto request, string normalizedEmail, string passwordHash, List<Permission> permissions) => new()
        {
            Id = Guid.NewGuid(),
            Email = normalizedEmail,
            PasswordHash = passwordHash,
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            IsOwner = request.IsOwner,
            Permissions = permissions,
        };

        public static string NormalizeEmail(string email) => email.Trim().ToLowerInvariant();
    }
}
