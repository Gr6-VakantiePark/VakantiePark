namespace VakantiePark.Shared.DTOs.Accommodations
{
    public class SearchOptionsDto
    {
        public List<string> Categories { get; set; } = new();

        public List<AccommodationTypeDto> Types { get; set; } = new();

        public List<AmenityOptionDto> Amenities { get; set; } = new();

        public int MaxPersons { get; set; }

        public const int DefaultTouristTaxMinimumAge = 3;

        public int MaxPets { get; set; }

        public int TouristTaxMinimumAge { get; set; } = DefaultTouristTaxMinimumAge;
    }

    public class AccommodationTypeDto
    {
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
    }

    public class AmenityOptionDto : AmenityDto
    {
        public List<string> Categories { get; set; } = new();
    }
}
