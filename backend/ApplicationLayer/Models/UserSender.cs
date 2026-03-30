using DomainLayer.Domain.Users;

namespace ApplicationLayer.Models
{
    public sealed class UserSender
    {
        public Guid Id { get; init; }

        public string Name { get; init; } = string.Empty;

        public string Email { get; init; } = string.Empty;

        public UserRole Role { get; init; } = UserRole.Standard;
    }
}
