using System.Threading.Tasks;
using DomainLayer.Domain;
using DomainLayer.Domain.Projects;

namespace ApplicationLayer.Repositories
{
    public interface IProjectRepository
    {
        // Salvar um projeto
        Task SaveAsync(ProjectBase project);
        // Obter um projeto por ID
        Task<IEnumerable<ProjectBase>> GetAllAsync();
    }
}