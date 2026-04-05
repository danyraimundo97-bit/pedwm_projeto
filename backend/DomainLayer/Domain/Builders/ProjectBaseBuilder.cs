namespace DomainLayer.Domain.Builders
{
    /// <summary>Shared fluent steps for <see cref="DomainLayer.Domain.Projects.ProjectBase"/> hierarchy.</summary>
    public abstract class ProjectBaseBuilder<TBuilder> where TBuilder : ProjectBaseBuilder<TBuilder>
    {
        protected Guid _id = Guid.NewGuid();
        protected string _title = string.Empty;
        protected DateTime _startDate;
        protected DateTime _endDate;

        public TBuilder WithId(Guid id)
        {
            _id = id;
            return (TBuilder)this;
        }

        public TBuilder WithTitle(string title)
        {
            _title = title;
            return (TBuilder)this;
        }

        public TBuilder WithDates(DateTime start, DateTime end)
        {
            _startDate = start;
            _endDate = end;
            return (TBuilder)this;
        }

        protected void EnsureTitleAndDates()
        {
            if (string.IsNullOrWhiteSpace(_title))
            {
                throw new InvalidOperationException("Call WithTitle(...) before Build().");
            }

            if (_endDate < _startDate)
            {
                throw new InvalidOperationException("End date must be on or after start date.");
            }
        }
    }
}
