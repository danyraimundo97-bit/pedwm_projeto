using DomainLayer.Domain.Projects;

namespace DomainLayer.Builders
{
    public class TrainingBuilder : ProjectBaseBuilder<TrainingBuilder>
    {
        private string _courseName = string.Empty;
        private double _trainingHours;

        public TrainingBuilder WhichCourse(string course)
        {
            _courseName = course;
            return this;
        }

        public TrainingBuilder WithDuration(double hours)
        {
            _trainingHours = hours;
            return this;
        }

        public Training Build()
        {
            return new Training
            {
                Title = _title,             // Inherited from ProjectBaseBuilder
                StartDate = _startDate,     // Inherited from ProjectBaseBuilder
                EndDate = _endDate,         // Inherited from ProjectBaseBuilder
                CourseName = _courseName,
                TrainingHours = _trainingHours
            };
        }
    }
}