namespace VakantiePark.Shared.DTOs.Afbeeldingen
{
    public class AfbeeldingBestandDto
    {
        public string Bestandsnaam { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public byte[] Data { get; set; } = [];
    }
}
