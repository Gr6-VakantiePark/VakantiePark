namespace VakantiePark.Domain.Entities
{
    public class PoolReservation
    {
        public Guid Id { get; set; }

        public Guid PoolTimeSlotId { get; set; }
        public PoolTimeSlot PoolTimeSlot { get; set; } = null!;

        public DateOnly Date { get; set; }

        public int Persons { get; set; }
    }
}
