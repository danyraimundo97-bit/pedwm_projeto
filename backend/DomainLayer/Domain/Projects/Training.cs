using DomainLayer.Domain.Builders;

namespace DomainLayer.Domain.Projects
{
    public class Training : ProjectBase
    {
        public string CourseName { get; private set; } = string.Empty;

        public string CertificationLink { get; private set; } = string.Empty;

        public double Hours { get; private set; }

        private Training()
        {
        }

        internal Training(
            Guid id,
            string title,
            DateTime startDate,
            DateTime endDate,
            string courseName,
            string certificationLink,
            double hours)
            : base(id, title, startDate, endDate)
        {
            CourseName = courseName;
            CertificationLink = certificationLink;
            Hours = hours;
        }

        public override double GetTotalAllocatedHours()
        {
            return Hours;
        }

        public static TrainingBuilder Builder() => new TrainingBuilder();
    }
}
