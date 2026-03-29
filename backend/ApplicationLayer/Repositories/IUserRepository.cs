using System.Collections.Generic;
using System.Threading.Tasks;
using DomainLayer.Domain.Users;

namespace ApplicationLayer.Repositories
{
    public interface IUserRepository
    {
        // Salvar um User
        Task SaveAsync(User user);

        // Obter todos os Users
        Task<IEnumerable<User>> GetAllAsync();
    }
}