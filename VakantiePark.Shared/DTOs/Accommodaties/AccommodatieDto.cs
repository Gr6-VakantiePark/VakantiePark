namespace VakantiePark.Shared.DTOs.Accommodaties
{
    /// <summary>Beknopte gegevens van een accommodatie, voor overzichten (GET /accommodaties).</summary>
    public class AccommodatieDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Naam { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string TypeSlug { get; set; } = string.Empty;

        /// <summary>"Huisje" of "Campingplaats".</summary>
        public string Categorie { get; set; } = string.Empty;

        public int MaxPersonen { get; set; }
        public string Zone { get; set; } = string.Empty;
        public decimal PrijsPerNacht { get; set; }

        /// <summary>Eerste afbeelding van het type, of null als er geen is.</summary>
        public string? Hoofdafbeelding { get; set; }
    }
}
