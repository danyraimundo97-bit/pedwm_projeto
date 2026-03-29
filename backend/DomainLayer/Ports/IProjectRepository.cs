using DomainLayer.Domain.Projects;

namespace DomainLayer.Ports
{
    /// <summary>Outbound port: persistence for <see cref="ProjectBase"/> (implemented in Infrastructure).</summary>
    public interface IProjectRepository
    {
        Task SaveAsync(ProjectBase project);

        Task<IReadOnlyList<ProjectBase>> GetAllAsync();
    }
}
