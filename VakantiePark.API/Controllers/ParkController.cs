using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VakantiePark.Application.Interfaces;
using VakantiePark.Shared.Common;
using VakantiePark.Shared.DTOs.Park;

namespace VakantiePark.API.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("park")]
    public class ParkController(IParkService service) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType<ApiResponse<ParkInfoDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ApiResponse<object>>(StatusCodes.Status404NotFound)]
        public Task<IActionResult> GetInfo() =>
            this.RunAsync(() => service.GetInfoAsync());
    }
}
