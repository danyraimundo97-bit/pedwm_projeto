using DomainLayer.Domain.Projects;

namespace ApplicationLayer.Repositories
{
    /// <summary>Outbound port: persistence for <see cref="ProjectBase"/> (implemented in Infrastructure).</summary>
    public interface IProjectRepository
    {
        Task SaveAsync(ProjectBase project);

        Task<IReadOnlyList<ProjectBase>> GetAllAsync();
    }
}
