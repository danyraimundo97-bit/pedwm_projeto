using System;
using DomainLayer.Domain.Tasks;

namespace DomainLayer.Builders
{
    public class FeatureTaskBuilder : TaskBaseBuilder<FeatureTaskBuilder, FeatureTask>
    {
        private int _storyPoints = 0;

        public FeatureTaskBuilder WithStoryPoints(int points)
        {
            _storyPoints = points;
            return this;
        }

        public override FeatureTask Build()
        {
            return new FeatureTask
            {
                Title = _title,               // Inherited from TaskBaseBuilder
                Description = _description,   // Inherited from TaskBaseBuilder
                ProjectId = _projectId,       // Inherited from TaskBaseBuilder
                StoryPoints = _storyPoints,
                Status = DomainLayer.Domain.Tasks.TaskStatus.Todo
            };
        }
    }
}