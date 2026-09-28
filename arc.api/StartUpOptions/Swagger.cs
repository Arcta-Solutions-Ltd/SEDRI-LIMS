using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace arc.api.StartUpOptions
{
    public static class Swagger
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            return services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "Arc Api", Version = "v1" }); });
        }

        public static IApplicationBuilder UseSwaggerView(this IApplicationBuilder app)
        {
            app.UseSwagger();

            return app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Arc Api Version 1"); });
        }
    }
}
