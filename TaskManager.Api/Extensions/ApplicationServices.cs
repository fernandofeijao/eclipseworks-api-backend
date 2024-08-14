using Microsoft.Extensions.Caching.Memory;
using TaskManager.Application;

namespace TaskManager.Api
{
    public static class ApplicationServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IProjectService, ProjectService>();

            return services;
        }
    }
}
