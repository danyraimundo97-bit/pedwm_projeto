using DomainLayer.Domain.Users;

namespace ApplicationLayer.Repositories
{
    public interface IUserRepository
    {
        Task SaveAsync(User user);
        Task<User?> GetByIdAsync(Guid id);
        Task<IReadOnlyList<User>> GetPagedAsync(int page, int size);
    }
}