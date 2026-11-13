using VakantiePark.Domain.Enums;

namespace VakantiePark.Domain.Entities
{
    public class Reservation
    {
        public Guid Id { get; set; }
        public int BookingSequence { get; set; }
        public string BookingNumber { get; set; } = string.Empty;
        public Guid AccommodationId { get; set; }
        public Accommodation Accommodation { get; set; } = default!;
        public Guid GuestId { get; set; }
        public Guest Guest { get; set; } = default!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int NumberOfGuests { get; set; }
        public ReservationStatus Status { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
