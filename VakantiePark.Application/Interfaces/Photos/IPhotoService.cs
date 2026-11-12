using VakantiePark.Shared.DTOs.Photos;

namespace VakantiePark.Application.Interfaces
{
    public interface IPhotoService
    {
        Task<PhotoFileDto> GetFileAsync(Guid id);
    }
}
