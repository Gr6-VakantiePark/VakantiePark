namespace VakantiePark.Domain.Entities
{
    /// <summary>
    /// Eén fysieke verblijfseenheid in het park (een huisje of een campingplaats).
    /// Bron: "accommodations" in accommodaties.json.
    /// </summary>
    public class Accommodatie
    {
        public int Id { get; set; }

        /// <summary>Unieke code uit het oude systeem, bijv. "BK4-001".</summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>Bijv. "Berk 4 01".</summary>
        public string Naam { get; set; } = string.Empty;

        /// <summary>Capaciteit: maximaal aantal personen (JSON: "maxPersons").</summary>
        public int MaxPersonen { get; set; }

        /// <summary>Gebied in het park, bijv. "Waterkant", "Kampeerveld Noord".</summary>
        public string Zone { get; set; } = string.Empty;

        /// <summary>0 = geen huisdieren toegestaan.</summary>
        public int MaxHuisdieren { get; set; }

        // --- Alleen voor huisjes ---
        public int? Slaapkamers { get; set; }
        public int? Badkamers { get; set; }
        public int? Bedden { get; set; }

        // --- Alleen voor campingplaatsen ---
        public int? ElektriciteitAmpere { get; set; }
        public decimal? MaxVoertuigLengteMeters { get; set; }
        public decimal? MaxCaravanLengteMeters { get; set; }

        /// <summary>"grass", "reinforced_grass" of "paved".</summary>
        public string? Ondergrond { get; set; }

        /// <summary>"none", "low", "medium" of "high".</summary>
        public string? Schaduw { get; set; }

        /// <summary>"limited", "standard" of "good".</summary>
        public string? WifiDekking { get; set; }

        // --- Relaties ---
        public int AccommodatieTypeId { get; set; }
        public AccommodatieType AccommodatieType { get; set; } = null!;

        public ICollection<Voorziening> Voorzieningen { get; set; } = new List<Voorziening>();
    }
}
