using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VakantiePark.Shared.Common;

namespace VakantiePark.API.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : ControllerBase
    {
        [Route("/error")]
        public IActionResult HandleError()
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ApiResponse<object>.Fail(ApiMessages.Unexpected));
        }
    }
}
