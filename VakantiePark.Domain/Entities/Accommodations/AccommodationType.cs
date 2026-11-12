using VakantiePark.Domain.Enums;

namespace VakantiePark.Domain.Entities
{
    public class AccommodationType
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Slug { get; set; } = string.Empty;

        public AccommodationCategory Category { get; set; }

        public string? Description { get; set; }

        public decimal PricePerNight { get; set; }

        public ICollection<Photo> Photos { get; set; } = new List<Photo>();

        public ICollection<Accommodation> Accommodations { get; set; } = new List<Accommodation>();
    }
}
