using VakantiePark.Domain.Enums;

namespace VakantiePark.Domain.Entities
{
    public class AccommodatieType
    {
        public int Id { get; set; }

        public string Naam { get; set; } = string.Empty;

        public string Slug { get; set; } = string.Empty;

        public AccommodatieCategorie Categorie { get; set; }

        public string? Omschrijving { get; set; }

        public decimal PrijsPerNacht { get; set; }

        public ICollection<Afbeelding> Afbeeldingen { get; set; } = new List<Afbeelding>();

        public ICollection<Accommodatie> Accommodaties { get; set; } = new List<Accommodatie>();
    }
}
