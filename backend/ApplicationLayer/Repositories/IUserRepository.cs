using DomainLayer.Domain.Users;

namespace DomainLayer.Domain.Repositories
{
    public interface IUserRepository
    {
        // Salvar um User
        Task SaveAsync(User user);

        // Obter todos os Users
        Task<IEnumerable<User>> GetAllAsync();
    }
}