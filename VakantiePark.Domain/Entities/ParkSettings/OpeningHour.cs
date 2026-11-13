using VakantiePark.Domain.Enums;

namespace VakantiePark.Domain.Entities
{
    public class OpeningHour
    {
        public Guid Id { get; set; }

        public Facility Facility { get; set; }

        public string Label { get; set; } = string.Empty;

        public string Value { get; set; } = string.Empty;

        public bool ShowOnHome { get; set; }

        public int SortOrder { get; set; }
    }
}
