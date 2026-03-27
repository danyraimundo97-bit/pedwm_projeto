using DomainLayer.Domain.Tasks;

namespace DomainLayer.Builders
{
    // Recebe o tipo do Builder (TBuilder) e o tipo da Tarefa final (TTask)
    public abstract class TaskBaseBuilder<TBuilder, TTask>
        where TBuilder : TaskBaseBuilder<TBuilder, TTask>
        where TTask : TaskBase
    {
        protected string _title = string.Empty;
        protected string _description = string.Empty;
        protected Guid _projectId;

        public TBuilder WithTitle(string title)
        {
            _title = title;
            return (TBuilder)this; // O "cast" mágico do CRTP
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

        // Obrigamos todos os filhos a implementar o método Build
        public abstract TTask Build();
    }
}