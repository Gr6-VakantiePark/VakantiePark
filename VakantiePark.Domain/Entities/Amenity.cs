namespace VakantiePark.Domain.Entities
{
    public class Amenity
    {
        public int Id { get; set; }

        public string Code { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public ICollection<Accommodation> Accommodations { get; set; } = new List<Accommodation>();
    }
}
