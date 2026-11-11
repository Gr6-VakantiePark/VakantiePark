namespace VakantiePark.Domain.Entities
{
    public class Role
    {
        public int Id { get; set; }

        public string Code { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public ICollection<Permission> Permissions { get; set; } = new List<Permission>();

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
