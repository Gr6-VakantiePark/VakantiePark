using VakantiePark.Domain.Enums;

namespace VakantiePark.Domain.Entities
{
    /// <summary>
    /// Een type accommodatie, bijv. "Berk 4" (huisje) of "Tentplaats Basis" (campingplaats).
    /// Bron: types.json en het veld "type" in accommodaties.json.
    /// </summary>
    public class AccommodatieType
    {
        public int Id { get; set; }

        /// <summary>Bijv. "Berk 4", "Camperplaats XL".</summary>
        public string Naam { get; set; } = string.Empty;

        /// <summary>Bijv. "berk-4". Wordt gebruikt in URL's (zoek-en-boek?type=berk-4).</summary>
        public string Slug { get; set; } = string.Empty;

        /// <summary>Huisje of Campingplaats.</summary>
        public AccommodatieCategorie Categorie { get; set; }

        public string? Omschrijving { get; set; }

        /// <summary>Prijs per nacht. Staat niet in de JSON-export en moet nog aangeleverd worden.</summary>
        public decimal PrijsPerNacht { get; set; }

        /// <summary>
        /// Bestandsnamen/URL's van de afbeeldingen van dit type (bijv. "berk4.png", "berk4-1.png").
        /// De eerste afbeelding is de hoofdfoto.
        /// </summary>
        public List<string> Afbeeldingen { get; set; } = new();

        public ICollection<Accommodatie> Accommodaties { get; set; } = new List<Accommodatie>();
    }
}
