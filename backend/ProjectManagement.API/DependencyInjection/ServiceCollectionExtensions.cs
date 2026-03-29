using ApplicationLayer.Handlers;
using ApplicationLayer.Mapping;
using ApplicationLayer.Queries;
using ApplicationLayer.Strategy;
using DomainLayer.Ports;
using InfrastructureLayer.Data;
using InfrastructureLayer.Mapping;
using InfrastructureLayer.Patterns.Strategy;
using InfrastructureLayer.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

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

            services.AddTransient<ListProjectsQueryHandler>();
            services.AddTransient<CreateProjectHandler>();
            services.AddTransient<CreateTaskHandler>();

            return services;
        }
    }
}
