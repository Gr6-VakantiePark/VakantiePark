namespace VakantiePark.Shared.DTOs.Prices
{
    public class PriceDto
    {
        public int Nights { get; set; }
        public int Persons { get; set; }
        public decimal PricePerNight { get; set; }
        public decimal Stay { get; set; }
        public int WeekendNights { get; set; }
        public decimal WeekendSurchargePercentage { get; set; }
        public decimal WeekendSurcharge { get; set; }
        public int Pets { get; set; }
        public decimal PetFeePerNight { get; set; }
        public decimal PetFee { get; set; }
        public decimal CleaningFee { get; set; }
        public int YoungChildren { get; set; }
        public int TouristTaxMinimumAge { get; set; }
        public decimal TouristTaxPerPersonPerNight { get; set; }
        public decimal TouristTax { get; set; }
        public decimal Total { get; set; }
    }
}
