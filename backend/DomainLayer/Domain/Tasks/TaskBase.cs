namespace DomainLayer.Domain.Tasks
{
    /// <summary>TPH root for tasks. Use builders in <c>DomainLayer.Domain.Builders</c> for <see cref="BugTask"/> / <see cref="FeatureTask"/>.</summary>
    public abstract class TaskBase
    {
        public Guid Id { get; private set; }

        public string Title { get; private set; } = string.Empty;

        public string Description { get; private set; } = string.Empty;

        public TaskStatus Status { get; protected set; } = TaskStatus.Todo;

        public Guid ProjectId { get; private set; }

        public Guid? AssignedUserId { get; private set; }

        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        public DateTime? CompletedAt { get; protected set; }

        protected TaskBase()
        {
        }

        protected TaskBase(
            Guid id,
            string title,
            string description,
            TaskStatus status,
            Guid projectId,
            Guid? assignedUserId,
            DateTime createdAt,
            DateTime? completedAt)
        {
            Id = id;
            Title = title;
            Description = description;
            Status = status;
            ProjectId = projectId;
            AssignedUserId = assignedUserId;
            CreatedAt = createdAt;
            CompletedAt = completedAt;
        }

        public abstract void MarkAsCompleted();
    }
}
