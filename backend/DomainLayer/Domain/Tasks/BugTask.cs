using DomainLayer.Domain.Builders;

namespace DomainLayer.Domain.Tasks
{
    public class BugTask : TaskBase
    {
        public BugSeverity Severity { get; private set; }

        public string Environment { get; private set; } = string.Empty;

        private BugTask()
        {
        }

        internal BugTask(
            Guid id,
            string title,
            string description,
            Guid projectId,
            BugSeverity severity,
            string environment,
            TaskStatus status,
            Guid? assignedUserId)
            : base(id, title, description, status, projectId, assignedUserId, DateTime.UtcNow, null)
        {
            Severity = severity;
            Environment = environment;
        }


        public override void MarkAsCompleted()
        {
            Status = TaskStatus.Completed;
            CompletedAt = DateTime.UtcNow;
        }

        public static BugTaskBuilder Builder() => new BugTaskBuilder();
    }
}
