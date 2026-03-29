using InfrastructureLayer.Patterns.Singleton;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using PresentationLayer.DependencyInjection;
using PresentationLayer.GraphQL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddBackendServices();

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
