namespace VakantiePark.Shared.DTOs.Accommodations
{
    public class AccommodationDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string TypeSlug { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public int MaxPersons { get; set; }
        public string Zone { get; set; } = string.Empty;
        public decimal PricePerNight { get; set; }

        public string? MainPhoto { get; set; }
    }
}
