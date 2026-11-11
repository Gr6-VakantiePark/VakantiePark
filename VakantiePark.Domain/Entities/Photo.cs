namespace VakantiePark.Domain.Entities
{
    public class Afbeelding
    {
        public int Id { get; set; }

        public string Bestandsnaam { get; set; } = string.Empty;

        public string ContentType { get; set; } = string.Empty;

        public int Volgorde { get; set; }

        public int AccommodatieTypeId { get; set; }
        public AccommodatieType AccommodatieType { get; set; } = null!;

        public AfbeeldingBestand Bestand { get; set; } = null!;
    }
}
