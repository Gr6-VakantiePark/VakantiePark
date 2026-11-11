using VakantiePark.Application.Interfaces;
using VakantiePark.Shared.DTOs.Photos;

namespace VakantiePark.Application.Services
{
    public class PhotoService(IPhotoRepository repository) : IPhotoService
    {
        public async Task<PhotoFileDto> GetFileAsync(int id)
        {
            if (id < 1)
                throw new ArgumentException("Het id van de afbeelding moet minimaal 1 zijn.");

            var photo = await repository.GetWithFileAsync(id)
                ?? throw new KeyNotFoundException($"Afbeelding met id {id} is niet gevonden.");

            return new PhotoFileDto
            {
                FileName = photo.FileName,
                ContentType = photo.ContentType,
                Data = photo.File.Data,
            };
        }
    }
}
