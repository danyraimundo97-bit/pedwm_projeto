using System;
using System.Collections.Generic;
using DomainLayer.Domain.Users;

namespace DomainLayer.Domain.Teams
{
    public class Team
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;

        // A lista de utilizadores que pertencem a esta equipa
        public ICollection<User> Members { get; set; } = new List<User>();
    }
}