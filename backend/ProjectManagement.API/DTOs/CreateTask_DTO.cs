using DomainLayer.Domain.Tasks;

namespace PresentationLayer.DTOs
{
    public class CreateTask_DTO
    {
        public TaskType Type { get; set; } // "BUG" | "FEATURE"
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Guid ProjectId { get; set; }
        public Guid? AssignedUserId { get; set; }   // A tarefa pode ser criada sem um utilizador atribuído (null)

        // Propriedades Opcionais (Específicas de cada tipo)

        // Bug
        public string? Environment { get; set; }
        public BugSeverity? Severity { get; set; } // "LOW" | "MEDIUM" | "HIGH" | "CRITICAL"

        // Feature
        public int? StoryPoints { get; set; }
    }
}
