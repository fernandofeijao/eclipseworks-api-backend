using TaskManager.Application;
using TaskManager.Infrastructure;

namespace TaskManager.Api
{
    public static class Repositories
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services, string? connectionString)
        {
            services.AddScoped(_ => new DbSession(connectionString ?? string.Empty));
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddTransient<IProjectRepository, ProjectRepository>();

            return services;
        }
    }
}
