namespace VakantiePark.Domain.Entities
{
    public class PhotoFile
    {
        public Guid PhotoId { get; set; }

        public byte[] Data { get; set; } = [];

        public Photo Photo { get; set; } = null!;
    }
}
