using VakantiePark.Domain.Entities;

namespace VakantiePark.Application.Interfaces
{
    public interface IPhotoRepository
    {
        Task<Photo?> GetWithFileAsync(Guid id);
    }
}
