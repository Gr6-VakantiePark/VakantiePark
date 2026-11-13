namespace VakantiePark.Shared.DTOs.Accommodations
{
    public class AccommodationTypeDetailDto : AccommodationTypeSummaryDto
    {
        public string? Description { get; set; }
        public int? MinBedrooms { get; set; }
        public int? MaxBedrooms { get; set; }
        public int? MinBathrooms { get; set; }
        public int? MaxBathrooms { get; set; }
        public int? MinBeds { get; set; }
        public int? MaxBeds { get; set; }
        public int MaxPets { get; set; }
        public int? MinElectricityAmps { get; set; }
        public int? MaxElectricityAmps { get; set; }
        public decimal? MinLengthMeters { get; set; }
        public decimal? MaxLengthMeters { get; set; }
        public List<string> Surfaces { get; set; } = new();
        public List<TypeAmenityDto> Amenities { get; set; } = new();
        public List<ZoneDto> Zones { get; set; } = new();
    }

    public class TypeAmenityDto : AmenityDto
    {
        public int UnitCount { get; set; }
    }
}
