using System.Threading.Tasks;
using DomainLayer.Domain.Projects;

namespace ApplicationLayer.Repositories
{
    public interface IProjectRepository
    {
        // Salvar um Project
        Task SaveAsync(ProjectBase project);

        // Obter todos os Projects
        Task<IEnumerable<ProjectBase>> GetAllAsync();
    }
}