using Microsoft.AspNetCore.Mvc;
using VakantiePark.Shared.Common;

namespace VakantiePark.API
{
    public static class ApiResults
    {
        public static async Task<IActionResult> RunAsync<T>(this ControllerBase controller, Func<Task<T>> action, Func<T, string?>? message = null)
        {
            try
            {
                T value = await action();
                return controller.Ok(ApiResponse<T>.Ok(value, message?.Invoke(value)));
            }
            catch (ArgumentException exception)
            {
                return controller.BadRequest(ApiResponse<T>.Fail(exception.Message));
            }
            catch (KeyNotFoundException exception)
            {
                return controller.NotFound(ApiResponse<T>.Fail(exception.Message));
            }
            catch (UnauthorizedAccessException exception)
            {
                return controller.Unauthorized(ApiResponse<T>.Fail(exception.Message));
            }
        }

        public static async Task<IActionResult> RunAsync(this ControllerBase controller, Func<Task> action, string message)
        {
            try
            {
                await action();
                return controller.Ok(ApiResponse<object>.Ok(message: message));
            }
            catch (ArgumentException exception)
            {
                return controller.BadRequest(ApiResponse<object>.Fail(exception.Message));
            }
            catch (KeyNotFoundException exception)
            {
                return controller.NotFound(ApiResponse<object>.Fail(exception.Message));
            }
            catch (UnauthorizedAccessException exception)
            {
                return controller.Unauthorized(ApiResponse<object>.Fail(exception.Message));
            }
        }

        public static Task<IActionResult> RunAsEmployeeAsync<T>(this ControllerBase controller, Func<Guid, Task<T>> action, Func<T, string?>? message = null)
        {
            if (!controller.User.TryGetUserId(out Guid userId))
                return Task.FromResult<IActionResult>(controller.Unauthorized(ApiResponse<T>.Fail(ApiMessages.NotLoggedIn)));
            return controller.RunAsync(() => action(userId), message);
        }
    }
}
