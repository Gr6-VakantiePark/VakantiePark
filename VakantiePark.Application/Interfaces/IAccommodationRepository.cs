using VakantiePark.Domain.Entities;
using VakantiePark.Domain.Enums;
using VakantiePark.Shared.DTOs.Accommodations;

namespace VakantiePark.Application.Interfaces
{
    public interface IAccommodationRepository
    {
        Task<List<Accommodation>> GetAllAsync();

        Task<Accommodation?> GetByIdAsync(Guid id);

        Task<List<AccommodationType>> GetTypesAsync();

        Task<AccommodationType?> GetTypeBySlugAsync(string slug);

        Task<List<Amenity>> GetAmenitiesAsync();

        Task<Dictionary<Guid, List<AccommodationCategory>>> GetAmenityCategoriesAsync();

        Task<(int MaxPersons, int MaxPets)> GetMaximumsAsync();

        Task<(List<Accommodation> Items, int TotalCount)> SearchAsync(AccommodationSearchCriteria pagination);
    }
}
