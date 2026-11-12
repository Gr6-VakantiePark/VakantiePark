namespace VakantiePark.Domain.Entities
{
    public class Accommodation
    {
        public Guid Id { get; set; }

        public string Code { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public int MaxPersons { get; set; }

        public string Zone { get; set; } = string.Empty;

        public int MaxPets { get; set; }

        public int? Bedrooms { get; set; }
        public int? Bathrooms { get; set; }
        public int? Beds { get; set; }

        public int? ElectricityAmps { get; set; }
        public decimal? MaxVehicleLengthMeters { get; set; }
        public decimal? MaxCaravanLengthMeters { get; set; }

        public string? Surface { get; set; }

        public string? Shade { get; set; }

        public string? WifiCoverage { get; set; }

        public Guid AccommodationTypeId { get; set; }
        public AccommodationType AccommodationType { get; set; } = null!;

        public ICollection<Amenity> Amenities { get; set; } = new List<Amenity>();
    }
}
