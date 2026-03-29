using DomainLayer.Domain.Users; // Para aceder ao UserRole
using System;

namespace ApplicationLayer.Commands
{
    public class CreateUserCommand
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.Standard;

        // A equipa é opcional, por isso usamos Guid? (com ponto de interrogação)
        public Guid? TeamId { get; set; }
    }
}