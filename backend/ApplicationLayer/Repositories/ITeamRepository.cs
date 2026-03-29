using System.Collections.Generic;
using System.Threading.Tasks;
using DomainLayer.Domain.Teams;

namespace ApplicationLayer.Repositories
{
    public interface ITeamRepository
    {
        // Salvar uma Team
        Task SaveAsync(Team team);

        // Obter todas as Teams
        Task<IEnumerable<Team>> GetAllAsync();
    }
}