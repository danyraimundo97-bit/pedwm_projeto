using DomainLayer.Domain.Builders;

namespace DomainLayer.Domain.Tasks
{
    public class BugTask : TaskBase
    {
        public BugSeverity Severity { get; private set; }
        public string Environment { get; private set; } = string.Empty;
        public override TaskType Type => TaskType.Bug;

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
            //TaskType type,
            TaskStatus status)
            : base(id, title, description, TaskType.Bug, status, projectId, null, DateTime.UtcNow, null)
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
