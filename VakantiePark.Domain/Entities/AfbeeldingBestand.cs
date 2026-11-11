namespace VakantiePark.Domain.Entities
{
    public class AfbeeldingBestand
    {
        public int AfbeeldingId { get; set; }

        public byte[] Data { get; set; } = [];

        public Afbeelding Afbeelding { get; set; } = null!;
    }
}
