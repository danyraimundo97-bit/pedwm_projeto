using DomainLayer.Domain.Projects;

namespace DomainLayer.Domain.Builders
{
    /// <summary>Fluent builder for standard projects.</summary>
    public sealed class ProjectBuilder : ProjectBaseBuilder<ProjectBuilder>, IBuilder<Project>
    {
        private int _budgetHours;
        private Guid _managerId;
        private Guid _teamId;
        private string _clientName = string.Empty;
        private ProjectStatus _projectStatus = ProjectStatus.Active;

        public ProjectBuilder WithBudget(int hours)
        {
            _budgetHours = hours;
            return this;
        }

        public ProjectBuilder ManagedBy(Guid? managerId)
        {
            _managerId = managerId.GetValueOrDefault();
            return this;
        }

        public ProjectBuilder ForTeam(Guid? teamId)

        {
            _teamId = teamId.GetValueOrDefault();
            return this;
        }

        public ProjectBuilder WithClientName(string clientName)
        {
            _clientName = clientName;
            return this;
        }

        public ProjectBuilder WithStatus(ProjectStatus status)
        {
            _projectStatus = status;
            return this;
        }

        public Project Build()
        {
            EnsureTitleAndDates();
            if (_managerId == Guid.Empty)
            {
                throw new InvalidOperationException("Call ManagedBy(managerId) before Build().");
            }

            return new Project(
                _id,
                _title.Trim(),
                _startDate,
                _endDate,
                _budgetHours,
                _managerId,
                _teamId,
                _clientName,
                _projectStatus);
        }
    }
}
