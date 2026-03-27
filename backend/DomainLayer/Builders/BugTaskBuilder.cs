using DomainLayer.Domain.Tasks;

namespace DomainLayer.Builders
{
    public class BugTaskBuilder : TaskBaseBuilder<BugTaskBuilder, BugTask>
    {
        private BugSeverity _severity = BugSeverity.High; // Default value for severity
        private string _environment = "Production";       // Default value for environment

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
            return new BugTask
            {
                Title = _title,               // Inherited from TaskBaseBuilder
                Description = _description,   // Inherited from TaskBaseBuilder
                ProjectId = _projectId,       // Inherited from TaskBaseBuilder
                Severity = _severity,
                Environment = _environment,
                Status = DomainLayer.Domain.Tasks.TaskStatus.Todo      // Tarefa nova começa sempre em Todo
            };
        }
    }
}