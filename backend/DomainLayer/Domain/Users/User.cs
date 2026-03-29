using DomainLayer.Domain.Builders;

namespace DomainLayer.Domain
{
    /// <summary>Fluent construction via <see cref="UserBuilder"/>.</summary>
    public class User
    {
        public Guid Id { get; private set; }

        public string Name { get; private set; } = string.Empty;

        public string Email { get; private set; } = string.Empty;

        public UserRole Role { get; private set; } = UserRole.Standard;

        private User()
        {
        }

        internal User(Guid id, string name, string email, UserRole role)
        {
            Id = id;
            Name = name;
            Email = email;
            Role = role;
        }

        // A associação opcional a uma equipa (pode ser null)
        public Guid? TeamId { get; set; }

        public void ChangeRole(UserRole newRole)
        {
            Role = newRole;
        }

        public static UserBuilder Builder() => new UserBuilder();
    }
}
