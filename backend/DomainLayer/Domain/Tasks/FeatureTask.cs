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
            TaskStatus status,
            Guid? assignedUserId)
            : base(id, title, description, status, projectId, assignedUserId, DateTime.UtcNow, null)
        {
            StoryPoints = storyPoints;
        }

        public static FeatureTaskBuilder Builder() => new FeatureTaskBuilder();
    }
}
