using EipqLibrary.Admin.Infrastructure;
using Microsoft.AspNetCore.Builder;

namespace EipqLibrary.Admin.Extensions
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
