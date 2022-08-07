using Microsoft.AspNetCore.Builder;

namespace YogurtCleaning.Middleware;

public static class CustomExeptionHandlerMiddlewareExtensions
{
    public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CustomExeptionHandlerMiddleware>();
    }
}