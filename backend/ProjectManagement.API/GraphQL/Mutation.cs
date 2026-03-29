using ApplicationLayer.Handlers;
using HotChocolate;
using PresentationLayer.DTOs;
using PresentationLayer.GraphQL.Mapping;

namespace PresentationLayer.GraphQL
{
    public class Mutation
    {
        public async Task<string> CreateProject(
            CreateProject_DTO input,
            [Service] CreateProjectHandler handler)
        {
            var command = input.ToCommand();
            var projectDTO = await handler.HandleAsync(command);
            return $"Projeto '{projectDTO.Title}' criado com o ID: {projectDTO.Id}";
        }

        public async Task<string> CreateTask(
            CreateTask_DTO input,
            [Service] CreateTaskHandler handler)
        {
            var command = input.ToCommand();
            var task = await handler.HandleAsync(command);
            return $"Tarefa '{task.Title}' criada com o ID: {task.Id}";
        }
    }
}
