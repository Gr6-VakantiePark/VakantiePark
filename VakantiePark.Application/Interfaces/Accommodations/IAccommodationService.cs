using VakantiePark.Shared.Common.Pagination;
using VakantiePark.Shared.DTOs.Accommodations;

namespace VakantiePark.Application.Interfaces
{
    public interface IAccommodationService
    {
        Task<List<AccommodationDto>> GetAllAsync();

        Task<AccommodationDetailDto> GetByIdAsync(Guid id, DateOnly? arrival = null, DateOnly? departure = null, int? persons = null, int youngChildren = 0, int pets = 0);

        Task<SearchOptionsDto> GetSearchOptionsAsync();

        Task<PaginatedResult<AccommodationDetailDto>> SearchAsync(AccommodationSearchCriteria criteria);
    }
}
