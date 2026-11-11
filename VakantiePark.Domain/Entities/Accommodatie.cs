namespace VakantiePark.Domain.Entities
{
    public class Accommodatie
    {
        public int Id { get; set; }

        public string Code { get; set; } = string.Empty;

        public string Naam { get; set; } = string.Empty;

        public int MaxPersonen { get; set; }

        public string Zone { get; set; } = string.Empty;

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

        public int AccommodatieTypeId { get; set; }
        public AccommodatieType AccommodatieType { get; set; } = null!;

        public ICollection<Voorziening> Voorzieningen { get; set; } = new List<Voorziening>();
    }
}
