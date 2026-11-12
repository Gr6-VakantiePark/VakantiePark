using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VakantiePark.Application.Interfaces.Booking;
using VakantiePark.Shared.Common;
using VakantiePark.Shared.DTOs.Reservations;

namespace VakantiePark.API.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("[controller]")]
    public class BookingController(IBookingService bookingService) : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType<ApiResponse<BookingDto>>(StatusCodes.Status201Created)]
        [ProducesResponseType<ApiResponse<BookingDto>>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<ApiResponse<BookingDto>>(StatusCodes.Status404NotFound)]
        public Task<IActionResult> Create([FromBody] CreateBookingDto request) =>
            this.RunCreatedAsync(() => bookingService.CreateAsync(request));
    }
}
