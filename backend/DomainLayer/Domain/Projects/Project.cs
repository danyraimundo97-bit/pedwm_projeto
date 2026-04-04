using DomainLayer.Domain.Builders;

namespace DomainLayer.Domain.Projects
{
    public class Project : ProjectBase
    {
        public double AllocatedHours { get; private set; }

        /// <summary>Hours logged / consumed against this project (budget is <see cref="AllocatedHours"/>).</summary>
        public double ConsumedHours { get; private set; }

        public ProjectStatus Status { get; private set; } = ProjectStatus.Active;

        public string ClientName { get; private set; } = string.Empty;

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
            ProjectStatus projectStatus,
            double consumedHours = 0)
            : base(id, title, startDate, endDate)
        {
            AllocatedHours = budgetHours;
            ConsumedHours = consumedHours;
            ManagerId = managerId;
            TeamId = teamId;
            ClientName = clientName;
            Status = projectStatus;
        }

        public void AddConsumedHours(double hours)
        {
            if (hours <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(hours), "Hours must be positive.");
            }

            ConsumedHours += hours;
        }

        public void ChangeStatus(ProjectStatus newStatus)
        {
            Status = newStatus;
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
