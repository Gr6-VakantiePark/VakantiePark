namespace VakantiePark.Domain.Entities
{
    public class Photo
    {
        public Guid Id { get; set; }

        public string FileName { get; set; } = string.Empty;

        public string ContentType { get; set; } = string.Empty;

        public int SortOrder { get; set; }

        public Guid AccommodationTypeId { get; set; }
        public AccommodationType AccommodationType { get; set; } = null!;

        public PhotoFile File { get; set; } = null!;
    }
}
