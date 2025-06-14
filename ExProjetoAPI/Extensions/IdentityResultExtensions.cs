using ExProjetoAPI.Responses;
using Microsoft.AspNetCore.Identity;

namespace ExProjetoAPI.Extensions
{
    public static class IdentityResultExtensions
    {
        public static List<ApiError> ToApiErrors(this IdentityResult result)
        {
            return result.Errors
                .Select(e => new ApiError
                {
                    Field = e.Code,
                    Message = e.Description
                })
                .ToList();
        }
    }
}