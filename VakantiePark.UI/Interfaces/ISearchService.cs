using VakantiePark.Shared.Common;
using VakantiePark.Shared.DTOs.Accommodations;

namespace VakantiePark.UI.Interfaces
{
    public interface ISearchService
    {
        Task<ApiResponse<SearchOptionsDto>> GetSearchOptionsAsync();

        Task<ApiResponse<PaginatedResult<AccommodationDetailDto>>> SearchAsync(AccommodationSearchCriteria criteria);
    }
}
