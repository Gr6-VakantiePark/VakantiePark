using VakantiePark.Shared.Common;

namespace VakantiePark.UI.Services
{
    public record ApiResult<T>(bool Succeeded, T? Value, string Message)
    {
        public static ApiResult<T> From(ApiResponse<T> response, string fallback) =>
            new(response.Succeeded, response.Value, string.IsNullOrWhiteSpace(response.Message) ? (response.Succeeded ? "" : fallback) : response.Message);
    }
}
