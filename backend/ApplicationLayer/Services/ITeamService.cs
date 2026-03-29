using System.Threading.Tasks;
using ApplicationLayer.Commands;
using DomainLayer.Domain.Teams;

namespace ApplicationLayer.Services
{
    // Interface: Contrato que define o que faz o TeamService
    public interface ITeamService
    {
        Task<Team> CreateTeamAsync(CreateTeamCommand command);
    }
}