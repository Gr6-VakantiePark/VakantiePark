using VakantiePark.Domain.Entities;

namespace VakantiePark.Domain.Interfaces
{
    public interface IPhotoRepository
    {
        Task<Photo?> GetWithFileAsync(Guid id);
    }
}
