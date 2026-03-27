namespace DomainLayer.Domain.Tasks
{
    public class BugTask : TaskBase
    {
        public BugSeverity Severity { get; set; }
        public string Environment { get; set; } = string.Empty; // ex: "Produção", "Testes"

        public override void MarkAsCompleted()
        {
            Status = TaskStatus.Completed;
            CompletedAt = DateTime.UtcNow;


            // Se o bug for Crítico, a lógica de conclusão pode ser diferente (ex: exigir preenchimento de um relatório de incidente).
        }
    }
}