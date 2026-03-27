// Usings da nossa Arquitetura
using ApplicationLayer.Factories;
using ApplicationLayer.Handlers;
using ApplicationLayer.Repositories;
using ApplicationLayer.Strategy;
using InfrastructureLayer.Patterns.Singleton;
using InfrastructureLayer.Patterns.Strategy;
using InfrastructureLayer.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PresentationLayer.GraphQL;
using InfrastructureLayer.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddSingleton<NotificationSender>();

// Repositories
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();

// Handlers
builder.Services.AddTransient<CreateProjectHandler>();
builder.Services.AddTransient<CreateTaskHandler>();

// Configurar o Servidor GraphQL (HotChocolate)
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>();

var app = builder.Build();

// ==========================================================
//  CONFIGURAR O PIPELINE HTTP
// ==========================================================

app.UseHttpsRedirection();

LoggerService.Instance.Log("--- ARRANQUE DA API COM GRAPHQL ---");

// Ligar o endpoint do GraphQL
app.MapGraphQL();

app.Run();