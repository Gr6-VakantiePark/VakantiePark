using VakantiePark.Domain.Entities;
using VakantiePark.Domain.Enums;
using VakantiePark.Shared.DTOs.Accommodations;

namespace VakantiePark.Application.Interfaces
{
    public interface IAccommodationRepository
    {
        Task<List<Accommodation>> GetAllAsync();

        Task<Accommodation?> GetByIdAsync(int id);

        Task<List<AccommodationType>> GetTypesAsync();

        Task<AccommodationType?> GetTypeBySlugAsync(string slug);

        Task<List<Amenity>> GetAmenitiesAsync();

        Task<Dictionary<int, List<AccommodationCategory>>> GetAmenityCategoriesAsync();

        Task<(int MaxPersons, int MaxPets)> GetMaximumsAsync();

        Task<(List<Accommodation> Items, int TotalCount)> SearchAsync(AccommodationSearchCriteria pagination);
    }
}
