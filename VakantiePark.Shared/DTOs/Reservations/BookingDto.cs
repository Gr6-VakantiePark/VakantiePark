using VakantiePark.Shared.DTOs.Prices;

namespace VakantiePark.Shared.DTOs.Reservations
{
    public class BookingDto
    {
        public Guid Id { get; set; }
        public string BookingNumber { get; set; } = string.Empty;
        public Guid AccommodationId { get; set; }
        public string AccommodationName { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int NumberOfGuests { get; set; }
        public string Status { get; set; } = string.Empty;
        public PriceDto Price { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public GuestDto Guest { get; set; } = new();
    }
}
