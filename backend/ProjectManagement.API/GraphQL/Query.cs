using ApplicationLayer.Repositories;
using DomainLayer.Domain;
using DomainLayer.Domain.Projects;
using HotChocolate;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PresentationLayer.GraphQL
{
    public class Query
    {
        // Query Simples
        public string BemVindo() => "API de Gestão de Projetos Online";

        // O [Service] injeta o nosso Repositório automaticamente
        public async Task<IEnumerable<ProjectBase>> GetProjects([Service] IProjectRepository repository)
        {
            return await repository.GetAllAsync();
        }
    }
}