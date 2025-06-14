namespace ExProjetoAPI.Responses
{
    public class ApiPagedResponse<T> : ApiResponse<List<T>>
    {
        public PaginationMetadata Meta { get; protected set; } = new();

        public static ApiPagedResponse<T> Ok(List<T> data, int totalItems, int page, int pageSize, string? message = null)
        {
            return new ApiPagedResponse<T>
            {
                Success = true,
                Data = data,
                Message = message,
                Meta = new PaginationMetadata
                {
                    TotalItems = totalItems,
                    Page = page,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
                }
            };
        }
    }
}
