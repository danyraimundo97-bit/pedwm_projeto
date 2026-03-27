namespace DomainLayer.Domain.Tasks
{
    public abstract class TaskBase
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TaskStatus Status { get; set; } = TaskStatus.Todo;

        // Relacionamentos cruciais
        public Guid ProjectId { get; set; } // A que projeto pertence esta tarefa?
        public Guid? AssignedUserId { get; set; } // Quem está a fazê-la? (Pode ser nulo se não estiver atribuída)

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedAt { get; set; }

        // Cada tipo de tarefa pode ter regras diferentes ao ser concluída.
        public abstract void MarkAsCompleted();
    }
}