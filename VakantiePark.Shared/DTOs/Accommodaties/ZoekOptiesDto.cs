namespace VakantiePark.Shared.DTOs.Accommodaties
{
    public class ZoekOptiesDto
    {
        public List<string> Categorieen { get; set; } = new();

        public List<AccommodatieTypeDto> Types { get; set; } = new();

        public List<VoorzieningOptieDto> Voorzieningen { get; set; } = new();

        public int MaxPersonen { get; set; }

        public int MaxHuisdieren { get; set; }
    }

    public class AccommodatieTypeDto
    {
        public string Naam { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Categorie { get; set; } = string.Empty;
    }

    public class VoorzieningOptieDto : VoorzieningDto
    {
        public List<string> Categorieen { get; set; } = new();
    }
}
