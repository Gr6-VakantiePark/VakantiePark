using VakantiePark.Domain.Entities;

namespace VakantiePark.Application.Interfaces
{
    public interface IAccommodatieRepository
    {
        /// <summary>Alle accommodaties inclusief hun type.</summary>
        Task<List<Accommodatie>> GetAllAsync();

        /// <summary>Eén accommodatie inclusief type en voorzieningen, of null als die niet bestaat.</summary>
        Task<Accommodatie?> GetByIdAsync(int id);

        Task<AccommodatieType?> GetTypeBySlugAsync(string slug);
    }
}
