using DomainLayer.Domain.Builders;

namespace DomainLayer.Domain.Tasks
{
    public class FeatureTask : TaskBase
    {
        public int StoryPoints { get; private set; }

        private FeatureTask()
        {
        }

        internal FeatureTask(
            Guid id,
            string title,
            string description,
            Guid projectId,
            int storyPoints,
            TaskStatus status)
            : base(id, title, description, status, projectId, null, DateTime.UtcNow, null)
        {
            StoryPoints = storyPoints;
        }

        public override void MarkAsCompleted()
        {
            Status = TaskStatus.Completed;
            CompletedAt = DateTime.UtcNow;
        }

        public static FeatureTaskBuilder Builder() => new FeatureTaskBuilder();
    }
}
