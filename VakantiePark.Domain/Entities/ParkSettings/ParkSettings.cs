namespace VakantiePark.Domain.Entities
{
    public class ParkSettings
    {
        public Guid Id { get; set; }

        public decimal CleaningFee { get; set; }

        public decimal TouristTaxPerPersonPerNight { get; set; }

        public int TouristTaxMinimumAge { get; set; }

        public decimal WeekendSurchargePercentage { get; set; }

        public decimal PetFeePerNight { get; set; }

        public TimeOnly ArrivalTime { get; set; }

        public TimeOnly DepartureTime { get; set; }

        public int MinimumNightsHighSeason { get; set; }

        public string Street { get; set; } = string.Empty;

        public string PostalCode { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
    }
}
