namespace VakantiePark.Shared.DTOs.Reservations
{
    public class CreateBookingDto
    {
        public Guid AccommodationId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int NumberOfGuests { get; set; }
        public decimal ExpectedTotalPrice { get; set; }
        public bool HasPaid { get; set; }
        public GuestDto Guest { get; set; } = new();
    }
}
