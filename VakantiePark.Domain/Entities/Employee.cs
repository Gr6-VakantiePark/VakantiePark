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

        public DateTime CreatedAt { get; set; }

        public DateTime? LastLoginAt { get; set; }

        public ICollection<Role> Roles { get; set; } = new List<Role>();

        public IEnumerable<Permission> Permissions =>
            Roles.SelectMany(role => role.Permissions).DistinctBy(permission => permission.Code);

        public bool HasPermission(string code) =>
            IsActive && Permissions.Any(permission => permission.Code == code);
    }
}
