using System;
using DomainLayer.Domain.Users; // Para o UserRole

namespace PresentationLayer.DTOs
{
    public class CreateUser_DTO
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.Standard;

        public Guid? TeamId { get; set; }
    }
}