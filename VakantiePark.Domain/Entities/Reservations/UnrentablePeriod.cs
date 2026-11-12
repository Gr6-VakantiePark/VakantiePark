using System;

namespace VakantiePark.Domain.Entities
{
    public class UnrentablePeriod
    {
        public Guid Id { get; set; }
        public Guid AccommodationId { get; set; }
        public Accommodation Accommodation { get; set; } = default!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; } = default!;
    }
}
