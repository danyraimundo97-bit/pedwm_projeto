using DomainLayer.Domain.Builders;

namespace DomainLayer.Domain.Tasks
{
    public class FeatureTask : TaskBase
    {
        public int StoryPoints { get; private set; }
        public override TaskType Type => TaskType.Feature;

        private FeatureTask()
        {
        }

        internal FeatureTask(
            Guid id,
            string title,
            string description,
            Guid projectId,
            int storyPoints,
            //TaskType type,
            TaskStatus status)
            : base(id, title, description, TaskType.Feature, status, projectId, null, DateTime.UtcNow, null)
        {
            StoryPoints = storyPoints;
        }

        public static FeatureTaskBuilder Builder() => new FeatureTaskBuilder();
    }
}
