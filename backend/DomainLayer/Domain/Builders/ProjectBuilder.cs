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
        private string _projectStatus = "Active";

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

        public ProjectBuilder ForTeam(Guid teamId)
        {
            _teamId = teamId;
            return this;
        }

        public ProjectBuilder WithClientName(string clientName)
        {
            _clientName = clientName;
            return this;
        }

        public ProjectBuilder WithStatus(string status)
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
                Guid.NewGuid(),
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
