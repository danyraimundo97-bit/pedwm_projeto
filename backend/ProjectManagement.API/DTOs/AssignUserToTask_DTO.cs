namespace PresentationLayer.DTOs
{
    public class AssignUserToTask_DTO
    {
        public string ProjectId { get; set; } = string.Empty;

        public string TaskId { get; set; } = string.Empty;

        public string AssigneeUserId { get; set; } = string.Empty;
    }
}
