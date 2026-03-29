using DomainLayer.Domain.Projects;

namespace DomainLayer.Builders
{
    // Herdamos do BaseBuilder, passando-lhe o ProjectBuilder como tipo!
    public class ProjectBuilder : ProjectBaseBuilder<ProjectBuilder>
    {
        private double _allocatedHours;
        private Guid _managerId;

        public ProjectBuilder WithBudget(double hours)
        {
            _allocatedHours = hours;
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
                AllocatedHours = _allocatedHours,
                ManagerId = _managerId
            };
        }
    }
}