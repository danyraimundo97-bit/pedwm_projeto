namespace DomainLayer.Domain.Projects
{
    public class Project : ProjectBase
    {
        public int BudgetHours { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public string ProjectStatus { get; set; } = "Active";

        // Ligações (Chaves Estrangeiras)
        public Guid ManagerId { get; set; }
        public Guid TeamId { get; set; }

        public override double GetTotalAllocatedHours()
        {
            // Implementação simples para calcular os dias
            return BudgetHours;
        }
    }
}