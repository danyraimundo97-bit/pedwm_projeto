using ApplicationLayer.Repositories;
using DomainLayer.Domain;
using DomainLayer.Domain.Projects;
using DomainLayer.Domain.Tasks;
using HotChocolate;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PresentationLayer.GraphQL
{
    public class Query
    {
        // Query Simples
        public string BemVindo() => "API de Gestão de Projetos Online";

        public async Task<IEnumerable<ProjectBase>> GetProjects([Service] IProjectRepository repository)
        {
            return await repository.GetAllAsync();
        }

        public async Task<IEnumerable<TaskBase>> GetTasks([Service] ITaskRepository repository)
        {
            return await repository.GetAllAsync();
        }
    }
}