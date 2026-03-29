using DomainLayer.Domain.Builders;

namespace DomainLayer.Domain.Projects
{
    public class SickLeave : ProjectBase
    {
        public string? MedicalCertificateId { get; private set; }

        public bool IsPaid { get; private set; }

        public double MissedHours { get; private set; }

        private SickLeave()
        {
        }

        internal SickLeave(
            Guid id,
            string title,
            DateTime startDate,
            DateTime endDate,
            double missedHours,
            string? medicalCertificateId,
            bool isPaid)
            : base(id, title, startDate, endDate)
        {
            MissedHours = missedHours;
            MedicalCertificateId = medicalCertificateId;
            IsPaid = isPaid;
        }

        public override double GetTotalAllocatedHours()
        {
            return MissedHours;
        }

        public static SickLeaveBuilder Builder() => new SickLeaveBuilder();
    }
}
