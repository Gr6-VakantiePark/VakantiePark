namespace VakantiePark.Shared.DTOs.Pool
{
    public class PoolTimeSlotDto
    {
        public Guid Id { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public int Capacity { get; set; }
        public int AvailablePlaces { get; set; }
        public bool IsFull { get; set; }
    }
}
