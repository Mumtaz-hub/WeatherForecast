using Api.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace Api.Extensions
{
    public static class ApplicationExtensions
    {
        public static void UseErrorHandlingMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionsHandlerMiddleware>();
        }
    }
}
