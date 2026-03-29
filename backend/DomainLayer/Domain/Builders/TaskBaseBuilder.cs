using DomainLayer.Domain.Tasks;

namespace DomainLayer.Domain.Builders
{
    /// <summary>Abstract builder for <see cref="TaskBase"/> derivatives (CRTP).</summary>
    public abstract class TaskBaseBuilder<TBuilder, TTask> : IBuilder<TTask>
        where TBuilder : TaskBaseBuilder<TBuilder, TTask>
        where TTask : TaskBase
    {
        protected string _title = string.Empty;
        protected string _description = string.Empty;
        protected Guid _projectId;

        public TBuilder WithTitle(string title)
        {
            _title = title;
            return (TBuilder)this;
        }

        public TBuilder WithDescription(string description)
        {
            _description = description;
            return (TBuilder)this;
        }

        public TBuilder InProject(Guid projectId)
        {
            _projectId = projectId;
            return (TBuilder)this;
        }

        public abstract TTask Build();

        protected void EnsureProjectContext()
        {
            if (_projectId == Guid.Empty)
            {
                throw new InvalidOperationException("Call InProject(projectId) before Build().");
            }
        }

        protected void EnsureTitle()
        {
            if (string.IsNullOrWhiteSpace(_title))
            {
                throw new InvalidOperationException("Call WithTitle(...) before Build().");
            }
        }
    }
}
