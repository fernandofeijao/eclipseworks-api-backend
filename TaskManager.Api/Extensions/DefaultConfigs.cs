using Asp.Versioning;

namespace TaskManager.Api
{
    public static class DefaultConfigs
    {
        private static readonly ApiVersion _apiVersion = new(1, 0);

        public static IServiceCollection AddBaseConfig(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();

            services.AddApiVersioning(o =>
            {
                o.ReportApiVersions = true;
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = _apiVersion;
            })
            .AddMvc()
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            return services;
        }

        public static WebApplication? UseSwaggerDocs(this WebApplication? app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("../swagger/v1/swagger.json", "API TaskManager");
                c.RoutePrefix = String.Empty;
            });

            return app;
        }
    }
}