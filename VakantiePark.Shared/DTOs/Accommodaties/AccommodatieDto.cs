namespace VakantiePark.Shared.DTOs.Accommodaties
{
    public class AccommodatieDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Naam { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string TypeSlug { get; set; } = string.Empty;

        public string Categorie { get; set; } = string.Empty;

        public int MaxPersonen { get; set; }
        public string Zone { get; set; } = string.Empty;
        public decimal PrijsPerNacht { get; set; }

        public string? Hoofdafbeelding { get; set; }
    }
}
