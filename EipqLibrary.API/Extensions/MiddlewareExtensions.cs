using EipqLibrary.API.Infrastructure;
using Microsoft.AspNetCore.Builder;

namespace EipqLibrary.API.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            return app;
        }
    }
}
