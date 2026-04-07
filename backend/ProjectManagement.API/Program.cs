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

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
    var userRepo = scope.ServiceProvider.GetRequiredService<IUserRepository>();
    SuperUserSeeder.EnsureExistsAsync(userRepo).GetAwaiter().GetResult();
}

app.UseCors("FlutterWebDev");
app.UseHttpsRedirection();

LoggerService.Instance.LogInfo("--- ARRANQUE DA API COM GRAPHQL ---");

app.MapHub<NotificationsHub>("/hubs/notifications");
app.MapGraphQL();

app.Run();
