namespace ExProjetoAPI.Responses
{
    public class ApiResponse<T>
    {
        public bool Success { get; protected set; }
        public T? Data { get; protected set; }
        public string? Message { get; protected set; }
        public IEnumerable<ApiError>? Errors { get; protected set; }

        public static ApiResponse<T> Ok(T data, string? message = null) =>
            new() { Success = true, Data = data, Message = message };

        public static ApiResponse<T> Ok(string? message = null) =>
            new() { Success = true, Message = message };

        public static ApiResponse<T> Fail(string message) =>
            new() { Success = false, Message = message };

        public static ApiResponse<T> Fail(IEnumerable<ApiError> errors, string? message = null) =>
            new() { Success = false, Errors = errors, Message = message };
    }
}