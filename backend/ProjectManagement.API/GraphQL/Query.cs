using ApplicationLayer.Models;
using ApplicationLayer.Queries;
using ApplicationLayer.Repositories;
using HotChocolate;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace PresentationLayer.GraphQL
{
    public class Query
    {
        public string BemVindo() => "API de Gestão de Projetos Online";

        public async Task<IReadOnlyList<ProjectDto>> GetProjects([Service] ListProjectsQueryHandler handler) //TODO: Modificar o objecto para ProjectBaseDTO
        {
            //TODO: Adicionar Paginação
            return await handler.HandleAsync();
        }

        public async Task<IEnumerable<TaskBase>> GetTasks([Service] ITaskRepository repository) //TODO: Modificar o objecto para TaskBaseDTO
        {
            //TODO: Adicionar Paginação
            return await repository.GetAllAsync();
        }
    }
}
