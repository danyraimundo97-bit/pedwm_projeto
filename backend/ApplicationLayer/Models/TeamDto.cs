using System;
using DomainLayer.Domain.Teams;

namespace ApplicationLayer.Models
{
    public sealed class TeamDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
    }
}