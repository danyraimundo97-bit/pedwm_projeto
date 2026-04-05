using System;
using System.Collections.Generic;
using DomainLayer.Domain.Teams;
using DomainLayer.Domain.Users;

namespace DomainLayer.Domain.Builders
{
    public sealed class TeamBuilder : IBuilder<Team>
    {
        private Guid _id = Guid.NewGuid();
        private string _name = string.Empty;
        private readonly List<User> _members = new List<User>();

        public TeamBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }

        public TeamBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        // Método para adicionar um membro de cada vez durante a construção
        public TeamBuilder WithMember(User user)
        {
            if (user != null)
            {
                _members.Add(user);
            }
            return this;
        }

        public Team Build()
        {
            if (string.IsNullOrWhiteSpace(_name))
            {
                throw new InvalidOperationException("Call WithName(...) before Build().");
            }

            return new Team
            {
                Id = _id,
                Name = _name.Trim(),
                Members = _members
            };
        }
    }
}