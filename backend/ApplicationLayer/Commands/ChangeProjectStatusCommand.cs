using DomainLayer.Domain.Projects;

namespace ApplicationLayer.Commands
{
    public class ChangeProjectStatusCommand
    {
        public string ProjectId { get; set; } = string.Empty;

        public ProjectStatus Status { get; set; }
    }
}
