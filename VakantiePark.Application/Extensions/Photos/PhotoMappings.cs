using VakantiePark.Domain.Entities;
using VakantiePark.Shared.DTOs.Photos;

namespace VakantiePark.Application.Extensions.Photos
{
    public static class PhotoMappings
    {
        public static PhotoFileDto ToFileDto(this Photo photo) => new()
        {
            FileName = photo.FileName,
            ContentType = photo.ContentType,
            Data = photo.File.Data,
        };
    }
}
