using System.Threading.Tasks;
using Mapster;
using ApplicationLayer.Commands;
using ApplicationLayer.Handlers;
using PresentationLayer.DTOs;

namespace PresentationLayer.GraphQL
{
    public class Mutation
    {
        // O [Service] diz ao GraphQL para ir buscar o nosso Handler à Injeção de Dependências!
        public async Task<string> CreateProject(
            CreateProject_DTO input,
            [Service] CreateProjectHandler handler)
        {
            // MAPSTER transforma o DTO num Command
            var command = input.Adapt<CreateProjectCommand>();

            // Usar o Handler
            var project = await handler.HandleAsync(command);

            return $"Projeto '{project.Title}' criado com o ID: {project.Id}";
        }

        public async Task<string> CreateTask(
            CreateTask_DTO input,
            [Service] CreateTaskHandler handler)
        {
            // MAPSTER transforma o DTO num Command
            var command = input.Adapt<CreateTaskCommand>();

            // Usar o Handler
            var task = await handler.HandleAsync(command);

            return $"Tarefa '{task.Title}' criada com o ID: {task.Id}";
        }
    }
}