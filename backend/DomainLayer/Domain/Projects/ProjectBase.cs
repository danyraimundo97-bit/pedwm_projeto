namespace DomainLayer.Domain.Projects
{
    /// <summary>Root for TPH project hierarchy. Construct via builders in <c>DomainLayer.Domain.Builders</c>.</summary>
    public abstract class ProjectBase
    {
        public Guid Id { get; private set; }

        public string Title { get; private set; } = string.Empty;

        public DateTime StartDate { get; private set; }

        public DateTime EndDate { get; private set; }

        protected ProjectBase()
        {
        }

        protected ProjectBase(Guid id, string title, DateTime startDate, DateTime endDate)
        {
            Id = id;
            Title = title;
            StartDate = startDate;
            EndDate = endDate;
        }

        public bool IsCurrentlyActive()
        {
            return DateTime.UtcNow >= StartDate && DateTime.UtcNow <= EndDate;
        }

        public abstract double GetTotalAllocatedHours();
    }
}
