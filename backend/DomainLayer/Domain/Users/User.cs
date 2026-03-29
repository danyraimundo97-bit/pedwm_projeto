namespace DomainLayer.Domain.Users
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.Standard;

        // A associação opcional a uma equipa (pode ser null)
        public Guid? TeamId { get; set; }

        public void ChangeRole(UserRole newRole)
        {
            Role = newRole;
        }
    }
}