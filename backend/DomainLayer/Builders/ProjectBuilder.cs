using DomainLayer.Domain.Projects;

namespace DomainLayer.Builders
{
    // Herdamos do BaseBuilder, passando-lhe o ProjectBuilder como tipo!
    public class ProjectBuilder : ProjectBaseBuilder<ProjectBuilder>
    {
        private int _budgetHours;
        private Guid _managerId;

        public ProjectBuilder WithBudget(int hours)
        {
            _budgetHours = hours;
            return this;
        }

        public ProjectBuilder ManagedBy(Guid managerId)
        {
            _managerId = managerId;
            return this;
        }

        // O método final que gera a entidade real
        public Project Build()
        {
            return new Project
            {
                Title = _title,           // Inherited from ProjectBaseBuilder
                StartDate = _startDate,   // Inherited from ProjectBaseBuilder
                EndDate = _endDate,       // Inherited from ProjectBaseBuilder
                BudgetHours = _budgetHours,
                ManagerId = _managerId,
                ProjectStatus = "Active"
            };
        }
    }
}