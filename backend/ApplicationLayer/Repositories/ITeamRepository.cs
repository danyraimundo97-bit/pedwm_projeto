using DomainLayer.Domain.Teams;

namespace ApplicationLayer.Repositories
{
    public interface ITeamRepository
    {
        // Salvar uma Team
        Task SaveAsync(Team team);

        // Obter todas as Teams
        Task<IReadOnlyList<Team>> GetAllAsync();
    }
}