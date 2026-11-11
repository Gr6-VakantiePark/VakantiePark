namespace VakantiePark.Shared.DTOs.Photos
{
    public class PhotoFileDto
    {
        public string FileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public byte[] Data { get; set; } = [];
    }
}
