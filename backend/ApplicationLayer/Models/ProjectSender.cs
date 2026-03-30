using DomainLayer.Domain.Projects;

namespace ApplicationLayer.Models
{
    public sealed class ProjectSender
    {
        public Guid Id { get; init; }

        public string Title { get; init; } = string.Empty;

        public DateTime StartDate { get; init; }

        public DateTime EndDate { get; init; }

        public ProjectType Type { get; init; }
    }
}
