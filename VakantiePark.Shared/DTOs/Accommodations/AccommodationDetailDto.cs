using VakantiePark.Shared.DTOs.Prices;

namespace VakantiePark.Shared.DTOs.Accommodations
{
    public class AccommodationDetailDto : AccommodationDto
    {
        public string? Description { get; set; }
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

        public List<string> Photos { get; set; } = new();
        public List<AmenityDto> Amenities { get; set; } = new();

        public PriceDto? Price { get; set; }
    }
}
