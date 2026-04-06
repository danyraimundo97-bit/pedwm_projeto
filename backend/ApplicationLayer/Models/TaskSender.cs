using DomainLayer.Domain.Tasks;
using TaskEntityStatus = DomainLayer.Domain.Tasks.TaskStatus;

namespace ApplicationLayer.Models
{
    /// <summary>GraphQL-facing task row (maps from <see cref="TaskBase"/> including Bug/Feature-specific fields).</summary>
    public sealed class TaskSender
    {
        public Guid Id { get; init; }

        public string Title { get; init; } = string.Empty;

        public string Description { get; init; } = string.Empty;

        public Guid ProjectId { get; init; }

        public TaskType TaskType { get; init; }

        public TaskEntityStatus Status { get; init; }

        public Guid? AssignedUserId { get; init; }

        public string? Environment { get; init; }

        public BugSeverity? Severity { get; init; }

        public int? StoryPoints { get; init; }
    }
}
