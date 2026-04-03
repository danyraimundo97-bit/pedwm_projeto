using System.Globalization;

namespace PresentationLayer.DTOs
{
    public class AssignUserToTask_DTO
    {
        string projectId {  get; set; }
        string taskId { get; set; }
        string assigneeUserId { get; set; }
    }
}
