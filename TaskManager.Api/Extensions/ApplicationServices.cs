using Microsoft.Extensions.Caching.Memory;
using TaskManager.Application;

namespace TaskManager.Api
{
    public static class ApplicationServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<IReportService, ReportService>();

            return services;
        }
    }
}
