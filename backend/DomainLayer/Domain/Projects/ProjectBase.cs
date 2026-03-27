namespace DomainLayer.Domain.Projects
{
    public abstract class ProjectBase
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public bool IsCurrentlyActive()
        {
            return DateTime.UtcNow >= StartDate && DateTime.UtcNow <= EndDate;
        }

        public abstract double GetTotalAllocatedHours();
    }
}