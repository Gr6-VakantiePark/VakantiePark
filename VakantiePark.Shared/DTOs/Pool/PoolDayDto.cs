namespace VakantiePark.Shared.DTOs.Pool
{
    public class PoolDayDto
    {
        public DateOnly Date { get; set; }

        public List<PoolTimeSlotDto> TimeSlots { get; set; } = new();
    }
}
