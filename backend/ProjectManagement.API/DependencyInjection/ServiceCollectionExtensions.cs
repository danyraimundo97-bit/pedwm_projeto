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
using Microsoft.Extensions.DependencyInjection;
using PresentationLayer.Notifications;

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

            services.AddSingleton<EmailDeliveryStrategy>();
            services.AddSingleton<SignalRNotificationDeliveryStrategy>();
            services.AddSingleton<INotificationDeliveryStrategy>(sp =>
                new CompositeNotificationDeliveryStrategy(
                    new INotificationDeliveryStrategy[]
                    {
                        sp.GetRequiredService<EmailDeliveryStrategy>(),
                        sp.GetRequiredService<SignalRNotificationDeliveryStrategy>(),
                    }));
            services.AddSingleton<INotificationService, NotificationService>();

            //MAPPER
            services.AddScoped<Mapper, DomainEntityDtoMapper>();

            // REPOSITORIES
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITeamRepository, TeamRepository>();
            services.AddScoped<IHourLogRepository, HourLogRepository>();

            services.AddTransient<ListProjectsQueryHandler>();
            services.AddTransient<ListTasksQueryHandler>();
            services.AddTransient<ListTeamsQueryHandler>();
            services.AddTransient<ListUsersQueryHandler>();

            services.AddTransient<GetProjectByIdQueryHandler>();
            services.AddTransient<GetProjectsByUserQueryHandler>();
            services.AddTransient<GetTaskByIdQueryHandler>();
            services.AddTransient<GetTasksByProjectQueryHandler>();
            services.AddTransient<GetTasksByUserQueryHandler>();
            services.AddTransient<GetTeamByIdQueryHandler>();
            services.AddTransient<GetUserByIdQueryHandler>();

            // MUTATIONS
            services.AddTransient<CreateProjectHandler>();
            services.AddTransient<CreateTaskHandler>();
            services.AddTransient<CreateUserHandler>();
            services.AddTransient<CreateTeamHandler>();
            services.AddTransient<AssignUserToTaskHandler>();
            services.AddTransient<AssignUserToTeamHandler>();
            services.AddTransient<AddHoursToProjectHandler>();
            services.AddTransient<ChangeProjectStatusHandler>();

            // SERVICES
            services.AddScoped<ISessionService, SessionService>();

            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITeamService, TeamService>();
            services.AddScoped<IAppLogger, AppLogger>();

            return services;
        }
    }
}
