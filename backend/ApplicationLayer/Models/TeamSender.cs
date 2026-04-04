using System;
using DomainLayer.Domain.Teams;

namespace ApplicationLayer.Models
{
    public sealed class TeamSender
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
    }
}