namespace DomainLayer.Domain.Projects
{
    public class Project : ProjectBase
    {
        public double AllocatedHours { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public ProjectStatus Status { get; set; } = ProjectStatus.Active;

        // Ligações (Chaves Estrangeiras)
        public Guid ManagerId { get; set; }
        public Guid TeamId { get; set; }

        public override double GetTotalAllocatedHours()
        {
            // Implementação simples para calcular os dias
            return AllocatedHours;
        }
    }
}