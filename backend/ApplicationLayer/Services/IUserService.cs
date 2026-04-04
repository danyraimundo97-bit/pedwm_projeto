using System.Threading.Tasks;
using ApplicationLayer.Commands;
using DomainLayer.Domain.Users;

namespace ApplicationLayer.Services
{
    // Interface: Contrato que define o que faz o UserService
    public interface IUserService
    {
        Task<User> CreateUserAsync(CreateUserCommand command);

        Task<User> AssignUserToTeamAsync(AssignUserToTeamCommand command);
    }
}