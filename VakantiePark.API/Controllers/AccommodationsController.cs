using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VakantiePark.Application.Interfaces;
using VakantiePark.Shared.Common;
using VakantiePark.Shared.DTOs.Accommodations;
using VakantiePark.Shared.DTOs.Prices;

namespace VakantiePark.API.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("accommodations")]
    public class AccommodationsController(IAccommodationService service, IPriceService priceService) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType<ApiResponse<List<AccommodationDto>>>(StatusCodes.Status200OK)]
        public Task<IActionResult> GetAll() =>
            this.RunAsync(() => service.GetAllAsync());

        [HttpGet("search-options")]
        [ProducesResponseType<ApiResponse<SearchOptionsDto>>(StatusCodes.Status200OK)]
        public Task<IActionResult> GetZoekOpties() =>
            this.RunAsync(() => service.GetSearchOptionsAsync());

        [HttpGet("search")]
        [ProducesResponseType<ApiResponse<PaginatedResult<AccommodationDetailDto>>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse<PaginatedResult<AccommodationDetailDto>>>(StatusCodes.Status400BadRequest)]
        public Task<IActionResult> Search([FromQuery] AccommodationSearchCriteria criteria) =>
            this.RunAsync(() => service.SearchAsync(criteria));

        [HttpGet("price")]
        [ProducesResponseType<ApiResponse<PriceDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse<PriceDto>>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ApiResponse<PriceDto>>(StatusCodes.Status404NotFound)]
        public Task<IActionResult> GetPrice([FromQuery] PriceRequestDto request) =>
            this.RunAsync(() => priceService.CalculateAsync(request));

        [HttpGet("{id:int}")]
        [ProducesResponseType<ApiResponse<AccommodationDetailDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse<AccommodationDetailDto>>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ApiResponse<AccommodationDetailDto>>(StatusCodes.Status404NotFound)]
        public Task<IActionResult> GetById(int id, [FromQuery] DateOnly? arrival, [FromQuery] DateOnly? departure, [FromQuery] int? persons) =>
            this.RunAsync(() => service.GetByIdAsync(id, arrival, departure, persons));
    }
}
