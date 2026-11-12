namespace VakantiePark.Domain.Entities
{
    public class Permission
    {
        public Guid Id { get; set; }

        public string Code { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public ICollection<Role> Roles { get; set; } = new List<Role>();
    }
}
