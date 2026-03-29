using InfrastructureLayer.Data;
using InfrastructureLayer.Patterns.Singleton;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PresentationLayer.DependencyInjection;
using PresentationLayer.GraphQL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddBackendServices();
// ==========================================================
// CONFIGURAÇÃO DA BASE DE DADOS (SQLite)
// ==========================================================
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=GestaoProjetos.db")); // O nome do ficheiro físico que vai ser criado!

// ==========================================================
//  INJEÇÃO DE DEPENDÊNCIAS
// ==========================================================

// Factories
builder.Services.AddSingleton<ProjectFactory>();
builder.Services.AddSingleton<ProjectTaskFactory>();

// Strategy (Notificações)
builder.Services.AddSingleton<INotificationDeliveryStrategy, EmailDeliveryStrategy>();
builder.Services.AddSingleton<INotificationDeliveryStrategy, WebSocketDeliveryStrategy>();
builder.Services.AddSingleton<NotificationSender>();

// Repositories
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITeamRepository, TeamRepository>();

// Handlers
builder.Services.AddTransient<CreateProjectHandler>();
builder.Services.AddTransient<CreateTaskHandler>();
builder.Services.AddTransient<CreateUserHandler>();
builder.Services.AddTransient<CreateTeamHandler>();

// Services
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITeamService, TeamService>();

// Logger
builder.Services.AddSingleton<IAppLogger, AppLogger>();

// Configurar o Servidor GraphQL (HotChocolate)
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("FlutterWebDev", policy =>
    {
        policy
            .SetIsOriginAllowed(origin =>
            {
                if (string.IsNullOrWhiteSpace(origin)) return false;
                var uri = new Uri(origin);
                return uri.Host == "localhost" || uri.Host == "127.0.0.1";
            })
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors("FlutterWebDev");
app.UseHttpsRedirection();

LoggerService.Instance.Log("--- ARRANQUE DA API COM GRAPHQL ---");

app.MapGraphQL();

app.Run();
