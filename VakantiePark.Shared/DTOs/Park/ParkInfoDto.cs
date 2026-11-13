namespace VakantiePark.Shared.DTOs.Park
{
    public class ParkInfoDto
    {
        public string ArrivalTime { get; set; } = string.Empty;
        public string DepartureTime { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int AccommodationsNearPool { get; set; }
        public List<OpeningHourDto> ReceptionHours { get; set; } = new();
        public List<OpeningHourDto> PoolHours { get; set; } = new();
        public List<PoolBasinDto> PoolBasins { get; set; } = new();
    }

    public class OpeningHourDto
    {
        public string Label { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public bool ShowOnHome { get; set; }
    }

    public class PoolBasinDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
    }
}
