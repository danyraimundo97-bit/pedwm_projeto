using ApplicationLayer.Repositories;
using DomainLayer.Domain.Builders;
using DomainLayer.Domain.Users;

namespace ApplicationLayer.Services
{
    public static class SuperUserSeeder
    {
        public static async Task EnsureExistsAsync(IUserRepository repository)
        {
            var existing = await repository.GetByIdAsync(SuperUser.Id);
            if (existing is not null)
                return;

            var user = new UserBuilder()
                .WithId(SuperUser.Id)
                .WithName(SuperUser.Name)
                .WithEmail(SuperUser.Email)
                .WithRole(UserRole.Admin)
                .Build();

            await repository.SaveAsync(user);
        }
    }
}
