using ApplicationLayer.Models;
using ApplicationLayer.Queries;
using HotChocolate;

namespace PresentationLayer.GraphQL
{
    public class Query
    {
        public string BemVindo() => "API de Gestão de Projetos Online";

        public async Task<IReadOnlyList<ProjectDto>> GetProjects([Service] ListProjectsQueryHandler handler)
        {
            return await handler.HandleAsync();
        }
    }
}
