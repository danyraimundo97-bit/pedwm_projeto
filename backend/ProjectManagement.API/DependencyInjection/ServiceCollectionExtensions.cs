using ApplicationLayer.Handlers;
using ApplicationLayer.Mapping;
using ApplicationLayer.Queries;
using ApplicationLayer.Repositories;
using ApplicationLayer.Services;
using InfrastructureLayer.Data;
using InfrastructureLayer.Mapping;
using InfrastructureLayer.Patterns.Singleton;
using InfrastructureLayer.Patterns.Strategy;
using InfrastructureLayer.Repositories;
using Microsoft.EntityFrameworkCore;

namespace PresentationLayer.DependencyInjection
{
    /// <summary>
    /// Composition root: registers application use cases, infrastructure adapters, and Mapster for domain→DTO mapping.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBackendServices(
            this IServiceCollection services,
            string sqliteConnectionString = "Data Source=GestaoProjetos.db")
        {
            MapsterConfiguration.Register();

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(sqliteConnectionString));

            services.AddSingleton<INotificationDeliveryStrategy, EmailDeliveryStrategy>();
            services.AddSingleton<NotificationSender>();

            services.AddScoped<IDomainEntityDtoMapper, DomainEntityDtoMapper>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITeamRepository, TeamRepository>();


            services.AddTransient<ListProjectsQueryHandler>();
            services.AddTransient<CreateProjectHandler>();
            services.AddTransient<CreateTaskHandler>();
            services.AddTransient<CreateUserHandler>();
            services.AddTransient<CreateTeamHandler>();

            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITeamService, TeamService>();
            services.AddScoped<IAppLogger, AppLogger>();

            return services;
        }
    }
}
