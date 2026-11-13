namespace VakantiePark.Domain.Entities
{
    public class PoolBasin
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Size { get; set; } = string.Empty;

        public int SortOrder { get; set; }
    }
}
