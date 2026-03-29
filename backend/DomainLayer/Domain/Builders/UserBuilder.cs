using DomainLayer.Domain;

namespace DomainLayer.Domain.Builders
{
    public sealed class UserBuilder : IBuilder<User>
    {
        private Guid _id = Guid.NewGuid();
        private string _name = string.Empty;
        private string _email = string.Empty;
        private UserRole _role = UserRole.Standard;

        public UserBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }

        public UserBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public UserBuilder WithEmail(string email)
        {
            _email = email;
            return this;
        }

        public UserBuilder WithRole(UserRole role)
        {
            _role = role;
            return this;
        }

        public User Build()
        {
            if (string.IsNullOrWhiteSpace(_name))
            {
                throw new InvalidOperationException("Call WithName(...) before Build().");
            }

            return new User(_id, _name.Trim(), _email.Trim(), _role);
        }
    }
}
