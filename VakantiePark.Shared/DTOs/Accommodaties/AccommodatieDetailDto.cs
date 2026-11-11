namespace VakantiePark.Shared.DTOs.Accommodaties
{
    /// <summary>Alle gegevens van één accommodatie (GET /accommodaties/{id}).</summary>
    public class AccommodatieDetailDto : AccommodatieDto
    {
        public string? Omschrijving { get; set; }
        public int MaxHuisdieren { get; set; }

        // Huisjes
        public int? Slaapkamers { get; set; }
        public int? Badkamers { get; set; }
        public int? Bedden { get; set; }

        // Campingplaatsen
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
