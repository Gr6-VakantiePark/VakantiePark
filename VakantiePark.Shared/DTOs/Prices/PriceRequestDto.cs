namespace VakantiePark.Shared.DTOs.Prices
{
    public class PriceRequestDto
    {
        public string Type { get; set; } = string.Empty;
        public DateOnly Arrival { get; set; }
        public DateOnly Departure { get; set; }
        public int Persons { get; set; }
    }
}
