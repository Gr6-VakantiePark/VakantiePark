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

        public Task<ApiResponse<PaginatedResult<AccommodationDetailDto>>> SearchAsync(AccommodationSearchCriteria criteria) =>
            http.GetAsync<PaginatedResult<AccommodationDetailDto>>($"accommodations/search?{criteria.ToQueryString()}");
    }
}
