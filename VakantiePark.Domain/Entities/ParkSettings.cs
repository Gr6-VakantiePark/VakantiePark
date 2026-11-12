namespace VakantiePark.Domain.Entities
{
    public class ParkSettings
    {
        public Guid Id { get; set; }

        public decimal CleaningFee { get; set; }

        public decimal TouristTaxPerPersonPerNight { get; set; }

        public TimeOnly ArrivalTime { get; set; }

        public TimeOnly DepartureTime { get; set; }

        public int MinimumNightsHighSeason { get; set; }
    }
}
