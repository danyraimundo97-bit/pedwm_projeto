using DomainLayer.Domain.Tasks;

namespace DomainLayer.Domain.Builders
{
    public sealed class BugTaskBuilder : TaskBaseBuilder<BugTaskBuilder, BugTask>
    {
        private BugSeverity _severity = BugSeverity.High;
        private string _environment = "Production";

        public BugTaskBuilder WithSeverity(BugSeverity severity)
        {
            _severity = severity;
            return this;
        }

        public BugTaskBuilder ForEnvironment(string environment)
        {
            _environment = environment;
            return this;
        }

        public override BugTask Build()
        {
            EnsureTitle();
            EnsureProjectContext();
            return new BugTask(
                Guid.NewGuid(),
                _title.Trim(),
                _description,
                _projectId,
                _severity,
                _environment,
                DomainLayer.Domain.Tasks.TaskStatus.Todo,
                _assignedUserId);
        }
    }
}
