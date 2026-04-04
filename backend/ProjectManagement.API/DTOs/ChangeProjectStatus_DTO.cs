using DomainLayer.Domain.Projects;

namespace PresentationLayer.DTOs
{
    public class ChangeProjectStatus_DTO
    {
        public string ProjectId { get; set; } = string.Empty;

        public ProjectStatus Status { get; set; }
    }
}
