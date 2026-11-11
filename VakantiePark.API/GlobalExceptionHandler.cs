using Microsoft.AspNetCore.Diagnostics;
using VakantiePark.Shared.Common;

namespace VakantiePark.API
{
    public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var (statusCode, message) = exception switch
            {
                ArgumentException => (StatusCodes.Status400BadRequest, exception.Message),
                KeyNotFoundException => (StatusCodes.Status404NotFound, exception.Message),
                UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, exception.Message),
                _ => (StatusCodes.Status500InternalServerError, ApiMessages.Unexpected),
            };

            if (statusCode == StatusCodes.Status500InternalServerError)
                logger.LogError(exception, "Onverwachte fout bij {Method} {Path}", httpContext.Request.Method, httpContext.Request.Path);

            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsJsonAsync(ApiResponse<object>.Fail(message), JsonDefaults.Options, cancellationToken);
            return true;
        }
    }
}
