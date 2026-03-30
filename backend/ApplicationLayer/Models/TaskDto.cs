using System;
using DomainLayer.Domain.Tasks;

namespace ApplicationLayer.Models
{
    public sealed class TaskDto
    {
        public Guid Id { get; init; }

        public string Title { get; init; } = string.Empty;

        public string Description { get; init; } = string.Empty;

        public Guid ProjectId { get; init; }

        public TaskType TaskType { get; init; }
    }
}
