using VakantiePark.Shared.Common;
using VakantiePark.Shared.Common.Pagination;
using VakantiePark.Shared.DTOs.Accommodations;
using VakantiePark.UI.Interfaces;

namespace VakantiePark.UI.Services
{
    public class ApiSearchService(IHttpService http) : ISearchService
    {
        public Task<ApiResponse<SearchOptionsDto>> GetSearchOptionsAsync() =>
            http.GetAsync<SearchOptionsDto>("accommodations/search-options");

        public Task<ApiResponse<List<AccommodationTypeSummaryDto>>> GetTypesAsync() =>
            http.GetAsync<List<AccommodationTypeSummaryDto>>("accommodations/types");

        public Task<ApiResponse<AccommodationTypeDetailDto>> GetTypeAsync(string slug) =>
            http.GetAsync<AccommodationTypeDetailDto>($"accommodations/types/{Uri.EscapeDataString(slug)}");

        public Task<ApiResponse<List<ZoneDto>>> GetZonesAsync() =>
            http.GetAsync<List<ZoneDto>>("accommodations/zones");

        public Task<ApiResponse<PaginatedResult<AccommodationDetailDto>>> SearchAsync(AccommodationSearchCriteria criteria) =>
            http.GetAsync<PaginatedResult<AccommodationDetailDto>>($"accommodations/search?{criteria.ToQueryString()}");
    }
}
