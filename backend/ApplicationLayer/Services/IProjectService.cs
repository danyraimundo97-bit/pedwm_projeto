using System.Threading.Tasks;
using ApplicationLayer.Commands;
using DomainLayer.Domain.Projects;

namespace ApplicationLayer.Services
{
    // Interface: Contrato que define o que faz o TeamService
    public interface IProjectService
    {
        Task<ProjectBase> CreateProjectAsync(CreateProjectCommand command);
    }
}