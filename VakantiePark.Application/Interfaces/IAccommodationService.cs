using VakantiePark.Shared.Common;
using VakantiePark.Shared.DTOs.Accommodations;

namespace VakantiePark.Application.Interfaces
{
    public interface IAccommodationService
    {
        Task<List<AccommodationDto>> GetAllAsync();

        Task<AccommodationDetailDto> GetByIdAsync(int id, DateOnly? arrival = null, DateOnly? departure = null, int? persons = null);

        Task<SearchOptionsDto> GetSearchOptionsAsync();

        Task<PaginatedResult<AccommodationDetailDto>> SearchAsync(AccommodationSearchCriteria criteria);
    }
}
