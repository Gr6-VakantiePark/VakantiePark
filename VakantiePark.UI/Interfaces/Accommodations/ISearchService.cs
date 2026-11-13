using VakantiePark.Shared.Common;
using VakantiePark.Shared.Common.Pagination;
using VakantiePark.Shared.DTOs.Accommodations;

namespace VakantiePark.UI.Interfaces
{
    public interface ISearchService
    {
        Task<ApiResponse<SearchOptionsDto>> GetSearchOptionsAsync();

        Task<ApiResponse<List<AccommodationTypeSummaryDto>>> GetTypesAsync();

        Task<ApiResponse<AccommodationTypeDetailDto>> GetTypeAsync(string slug);

        Task<ApiResponse<List<ZoneDto>>> GetZonesAsync();

        Task<ApiResponse<PaginatedResult<AccommodationDetailDto>>> SearchAsync(AccommodationSearchCriteria criteria);
    }
}
