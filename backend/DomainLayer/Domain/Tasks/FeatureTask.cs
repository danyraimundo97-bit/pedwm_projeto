namespace DomainLayer.Domain.Tasks
{
    public class FeatureTask : TaskBase
    {
        public int StoryPoints { get; set; }

        public override void MarkAsCompleted()
        {
            Status = TaskStatus.Completed;
            CompletedAt = DateTime.UtcNow;


            // Aqui podíamos adicionar lógica extra, como disparar um evento a dizer que a Feature X está pronta!
        }
    }
}