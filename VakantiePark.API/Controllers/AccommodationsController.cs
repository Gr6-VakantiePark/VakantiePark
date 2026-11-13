using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VakantiePark.Application.Interfaces;
using VakantiePark.Shared.Common;
using VakantiePark.Shared.Common.Pagination;
using VakantiePark.Shared.DTOs.Accommodations;
using VakantiePark.Shared.DTOs.Reservations;

namespace VakantiePark.API.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("accommodations")]
    public class AccommodationsController(IAccommodationService service, IAvailabilityService availabilityService) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType<ApiResponse<List<AccommodationDto>>>(StatusCodes.Status200OK)]
        public Task<IActionResult> GetAll() =>
            this.RunAsync(() => service.GetAllAsync());

        [HttpGet("search-options")]
        [ProducesResponseType<ApiResponse<SearchOptionsDto>>(StatusCodes.Status200OK)]
        public Task<IActionResult> GetZoekOpties() =>
            this.RunAsync(() => service.GetSearchOptionsAsync());

        [HttpGet("types")]
        [ProducesResponseType<ApiResponse<List<AccommodationTypeSummaryDto>>>(StatusCodes.Status200OK)]
        public Task<IActionResult> GetTypes() =>
            this.RunAsync(() => service.GetTypesAsync());

        [HttpGet("types/{slug}")]
        [ProducesResponseType<ApiResponse<AccommodationTypeDetailDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status404NotFound)]
        public Task<IActionResult> GetTypeBySlug(string slug) =>
            this.RunAsync(() => service.GetTypeBySlugAsync(slug));

        [HttpGet("zones")]
        [ProducesResponseType<ApiResponse<List<ZoneDto>>>(StatusCodes.Status200OK)]
        public Task<IActionResult> GetZones() =>
            this.RunAsync(() => service.GetZonesAsync());

        [HttpGet("search")]
        [ProducesResponseType<ApiResponse<PaginatedResult<AccommodationDetailDto>>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse<PaginatedResult<AccommodationDetailDto>>>(StatusCodes.Status400BadRequest)]
        public Task<IActionResult> Search([FromQuery] AccommodationSearchCriteria criteria) =>
            this.RunAsync(() => service.SearchAsync(criteria));

        [HttpGet("{id:guid}")]
        [ProducesResponseType<ApiResponse<AccommodationDetailDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse<AccommodationDetailDto>>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ApiResponse<AccommodationDetailDto>>(StatusCodes.Status404NotFound)]
        public Task<IActionResult> GetById(Guid id, [FromQuery] DateOnly? arrival, [FromQuery] DateOnly? departure, [FromQuery] int? persons, [FromQuery] int youngChildren = 0, [FromQuery] int pets = 0) =>
            this.RunAsync(() => service.GetByIdAsync(id, arrival, departure, persons, youngChildren, pets));

        [HttpGet("availability")]
        [ProducesResponseType<ApiResponse<bool>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse<bool>>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ApiResponse<bool>>(StatusCodes.Status404NotFound)]
        public Task<IActionResult> CheckAvailability([FromQuery] DateTime start, [FromQuery] DateTime end, [FromQuery] Guid accommodationId, [FromQuery] int amountOfPeople) =>
            this.RunAsync(() => availabilityService.CheckForPeriodAvailability(new ReservationAvailabilityRequest(start, end, accommodationId, amountOfPeople)));
    }
}
