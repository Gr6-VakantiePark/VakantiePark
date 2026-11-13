namespace VakantiePark.Shared.DTOs.Auth
{
    public class LoginRequestDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public EmployeeDto Employee { get; set; } = new();
    }

    public class EmployeeDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public bool IsOwner { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public List<string> Permissions { get; set; } = new();
        public List<string> PermissionNames { get; set; } = new();
    }

    public class UpdateEmployeeDto
    {
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }

    public class SetEmployeeActiveDto
    {
        public bool IsActive { get; set; }
    }

    public class UpdateEmployeePermissionsDto
    {
        public bool IsOwner { get; set; }
        public List<string> Permissions { get; set; } = new();
    }

    public class PermissionDto
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }

    public class CreateEmployeeDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public bool IsOwner { get; set; }
        public List<string> Permissions { get; set; } = new();
    }
}
