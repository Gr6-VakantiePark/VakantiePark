namespace VakantiePark.Domain.Entities
{
    public class Voorziening
    {
        public int Id { get; set; }

        public string Code { get; set; } = string.Empty;

        public string Naam { get; set; } = string.Empty;

        public ICollection<Accommodatie> Accommodaties { get; set; } = new List<Accommodatie>();
    }
}
