namespace VakantiePark.Domain.Entities
{
    public class PoolTimeSlot
    {
        public Guid Id { get; set; }

        public DayOfWeek DayOfWeek { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

        public int Capacity { get; set; }

        public ICollection<PoolReservation> Reservations { get; set; } = new List<PoolReservation>();

        public int AvailablePlaces(int reservedPlaces) =>
            Math.Max(0, Capacity - reservedPlaces);
    }
}
