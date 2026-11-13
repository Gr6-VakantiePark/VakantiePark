namespace VakantiePark.Domain.Entities
{
    public class Employee
    {
        public Guid Id { get; set; }

        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public bool IsOwner { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? LastLoginAt { get; set; }

        public ICollection<Permission> Permissions { get; set; } = new List<Permission>();

        public bool HasPermission(string code) =>
            IsActive && (IsOwner || Permissions.Any(permission => permission.Code == code));
    }
}
