namespace VakantiePark.Shared.Common
{
    public class ApiResponse<T>
    {
        public T? Value { get; set; }
        public bool Succeeded { get; set; }
        public string Message { get; set; } = string.Empty;

        public static ApiResponse<T> Ok(T? value = default, string? message = null)
            => new ApiResponse<T>() { Succeeded = true, Message = message ?? string.Empty, Value = value };

        public static ApiResponse<T> Fail(string? message = null)
            => new ApiResponse<T>() { Succeeded = false, Message = message ?? string.Empty };
    }
}
