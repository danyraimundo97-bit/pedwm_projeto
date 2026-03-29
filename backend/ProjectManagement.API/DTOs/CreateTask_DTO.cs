using ApplicationLayer.Models;

namespace PresentationLayer.DTOs
{
    public class CreateTask_DTO
    {
        public TaskType Type { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public Guid ProjectId { get; set; }

        public string? Environment { get; set; }

        public int? StoryPoints { get; set; }
    }
}
