using ApplicationLayer.Repositories;
using ApplicationLayer.Services;
using InfrastructureLayer.Data;
using InfrastructureLayer.Patterns.Singleton;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PresentationLayer.DependencyInjection;
using PresentationLayer.GraphQL;
using PresentationLayer.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();

// SQLite: project folder so Migrate() uses the same DB as dev (not cwd / bin-only copies).
var sqliteDbPath = System.IO.Path.Combine(builder.Environment.ContentRootPath, "GestaoProjetos.db");
builder.Services.AddBackendServices(sqliteConnectionString: $"Data Source={sqliteDbPath}");

// ==========================================================
//  INJEÇÃO DE DEPENDÊNCIAS (legado comentado — ver AddBackendServices)
// ==========================================================

// Factories
//builder.Services.AddSingleton<ProjectFactory>();
//builder.Services.AddSingleton<ProjectTaskFactory>();

//// Strategy (Notificações) — ativos via AddBackendServices: Email + SignalR (Composite).

//// Repositories
//builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
//builder.Services.AddScoped<ITaskRepository, TaskRepository>();
//builder.Services.AddScoped<IUserRepository, UserRepository>();
//builder.Services.AddScoped<ITeamRepository, TeamRepository>();

//// Handlers
//builder.Services.AddTransient<CreateProjectHandler>();
//builder.Services.AddTransient<CreateTaskHandler>();
//builder.Services.AddTransient<CreateUserHandler>();
//builder.Services.AddTransient<CreateTeamHandler>();

//// Services
//builder.Services.AddScoped<IProjectService, ProjectService>();
//builder.Services.AddScoped<ITaskService, TaskService>();
//builder.Services.AddScoped<IUserService, UserService>();
//builder.Services.AddScoped<ITeamService, TeamService>();

//// Logger
//builder.Services.AddSingleton<IAppLogger, AppLogger>();

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

// Migrações + utilizador super (admin) em Users
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    // Safety net: stale binary or wrong DB file can skip the HourLogs migration; idempotent on SQLite.
    if (db.Database.ProviderName?.Contains("Sqlite", StringComparison.OrdinalIgnoreCase) == true)
    {
        db.Database.ExecuteSqlRaw(
            """
            CREATE TABLE IF NOT EXISTS "HourLogs" (
                "Id" TEXT NOT NULL CONSTRAINT "PK_HourLogs" PRIMARY KEY,
                "ProjectId" TEXT NOT NULL,
                "TaskId" TEXT NULL,
                "Hours" REAL NOT NULL,
                "LoggedAtUtc" TEXT NOT NULL,
                "UserId" TEXT NOT NULL
            );
            """);
    }

    var userRepo = scope.ServiceProvider.GetRequiredService<IUserRepository>();
    SuperUserSeeder.EnsureExistsAsync(userRepo).GetAwaiter().GetResult();
}

app.UseCors("FlutterWebDev");
app.UseHttpsRedirection();

LoggerService.Instance.LogInfo("--- ARRANQUE DA API COM GRAPHQL ---");

app.MapHub<NotificationsHub>("/hubs/notifications");
app.MapGraphQL();

app.Run();
