namespace VakantiePark.Shared.DTOs.Prices
{
    public class PriceDto
    {
        public int Nights { get; set; }
        public int Persons { get; set; }
        public decimal PricePerNight { get; set; }
        public decimal Stay { get; set; }
        public decimal CleaningFee { get; set; }
        public decimal TouristTaxPerPersonPerNight { get; set; }
        public decimal TouristTax { get; set; }
        public decimal Total { get; set; }
    }
}
