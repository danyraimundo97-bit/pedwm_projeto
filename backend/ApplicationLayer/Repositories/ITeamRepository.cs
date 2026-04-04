using DomainLayer.Domain.Teams;

namespace ApplicationLayer.Repositories
{
    public interface ITeamRepository
    {
        Task SaveAsync(Team team);
        Task<Team?> GetByIdAsync(Guid id);
        Task<IReadOnlyList<Team>> GetPagedAsync(int page, int size);
    }
}