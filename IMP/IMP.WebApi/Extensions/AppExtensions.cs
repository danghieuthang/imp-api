using IMP.WebApi.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace IMP.WebApi.Extensions
{
    public static class AppExtensions
    {
        public static void UseSwaggerExtension(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            });
        }
        public static void UseReDocExtension(this IApplicationBuilder app)
        {
            app.UseReDoc(c =>
            {
                c.DocumentTitle = "REDOC API Documentation";
                c.SpecUrl = "/swagger/v1/swagger.json";
            });
        }
        public static void UseErrorHandlingMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandlerMiddleware>();
        }
    }
}
