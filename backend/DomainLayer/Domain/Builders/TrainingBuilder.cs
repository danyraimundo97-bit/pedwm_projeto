using DomainLayer.Domain.Projects;

namespace DomainLayer.Domain.Builders
{
    public sealed class TrainingBuilder : ProjectBaseBuilder<TrainingBuilder>, IBuilder<Training>
    {
        private string _courseName = string.Empty;
        private string _certificationLink = string.Empty;
        private double _hours;

        public TrainingBuilder WhichCourse(string course)
        {
            _courseName = course;
            return this;
        }

        public TrainingBuilder WithCertificationLink(string link)
        {
            _certificationLink = link;
            return this;
        }

        public TrainingBuilder WithDuration(double hours)
        {
            _hours = hours;
            return this;
        }

        public Training Build()
        {
            EnsureTitleAndDates();
            return new Training(
                Guid.NewGuid(),
                _title.Trim(),
                _startDate,
                _endDate,
                _courseName,
                _certificationLink,
                _hours);
        }
    }
}
