namespace DomainLayer.Domain
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.Standard;

        // Método do teu diagrama
        public void ChangeRole(UserRole newRole)
        {
            Role = newRole;
        }
    }
}