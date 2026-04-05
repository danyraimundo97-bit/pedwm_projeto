using DomainLayer.Domain.Tasks;

namespace DomainLayer.Domain.Builders
{
    public sealed class FeatureTaskBuilder : TaskBaseBuilder<FeatureTaskBuilder, FeatureTask>
    {
        private int _storyPoints;

        public FeatureTaskBuilder WithStoryPoints(int points)
        {
            _storyPoints = points;
            return this;
        }

        public override FeatureTask Build()
        {
            EnsureTitle();
            EnsureProjectContext();
            return new FeatureTask(
                _id,
                _title.Trim(),
                _description,
                _projectId,
                _storyPoints,
                DomainLayer.Domain.Tasks.TaskStatus.Todo,
                _assignedUserId);
        }
    }
}
