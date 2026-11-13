namespace VakantiePark.Shared.DTOs.Accommodations
{
    public class AccommodationTypeSummaryDto
    {
        public string Slug { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string? Tagline { get; set; }
        public int MinPersons { get; set; }
        public int MaxPersons { get; set; }
        public int UnitCount { get; set; }
    }
}
