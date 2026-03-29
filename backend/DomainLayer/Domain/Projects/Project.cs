using DomainLayer.Domain.Builders;

namespace DomainLayer.Domain.Projects
{
    public class Project : ProjectBase
    {
        public double AllocatedHours { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public ProjectStatus Status { get; set; } = ProjectStatus.Active;

        public string ClientName { get; private set; } = string.Empty;

        public string ProjectStatus { get; private set; } = "Active";

        public Guid ManagerId { get; private set; }

        public Guid TeamId { get; private set; }

        private Project()
        {
        }

        internal Project(
            Guid id,
            string title,
            DateTime startDate,
            DateTime endDate,
            int budgetHours,
            Guid managerId,
            Guid teamId,
            string clientName,
            string projectStatus)
            : base(id, title, startDate, endDate)
        {
            BudgetHours = budgetHours;
            ManagerId = managerId;
            TeamId = teamId;
            ClientName = clientName;
            ProjectStatus = projectStatus;
        }

        public override double GetTotalAllocatedHours()
        {
            // Implementação simples para calcular os dias
            return AllocatedHours;
        }

        /// <summary>Starts fluent construction; call <see cref="ProjectBuilder.Build"/> when complete.</summary>
        public static ProjectBuilder Builder() => new ProjectBuilder();
    }
}
