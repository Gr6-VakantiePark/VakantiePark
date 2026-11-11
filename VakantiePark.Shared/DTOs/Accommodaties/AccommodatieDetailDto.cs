namespace VakantiePark.Shared.DTOs.Accommodaties
{
    public class AccommodatieDetailDto : AccommodatieDto
    {
        public string? Omschrijving { get; set; }
        public int MaxHuisdieren { get; set; }

        public int? Slaapkamers { get; set; }
        public int? Badkamers { get; set; }
        public int? Bedden { get; set; }

        public int? ElektriciteitAmpere { get; set; }
        public decimal? MaxVoertuigLengteMeters { get; set; }
        public decimal? MaxCaravanLengteMeters { get; set; }
        public string? Ondergrond { get; set; }
        public string? Schaduw { get; set; }
        public string? WifiDekking { get; set; }

        public List<string> Afbeeldingen { get; set; } = new();
        public List<VoorzieningDto> Voorzieningen { get; set; } = new();
    }
}
