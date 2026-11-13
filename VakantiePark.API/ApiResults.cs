using Microsoft.AspNetCore.Mvc;
using VakantiePark.Shared.Common;

namespace VakantiePark.API
{
    public static class ApiResults
    {
        public static async Task<IActionResult> RunAsync<T>(this ControllerBase controller, Func<Task<T>> action, Func<T, string?>? message = null)
        {
            T value = await action();
            return controller.Ok(ApiResponse<T>.Ok(value, message?.Invoke(value)));
        }

        public static Task<IActionResult> RunAsEmployeeAsync<T>(this ControllerBase controller, Func<Guid, Task<T>> action, Func<T, string?>? message = null)
        {
            if (!controller.User.TryGetUserId(out Guid userId))
                return Task.FromResult<IActionResult>(controller.Unauthorized(ApiResponse<T>.Fail(ApiMessages.NotLoggedIn)));
            return controller.RunAsync(() => action(userId), message);
        }
    }
}
