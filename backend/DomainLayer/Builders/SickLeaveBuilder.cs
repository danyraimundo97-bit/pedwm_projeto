using DomainLayer.Domain.Projects;

namespace DomainLayer.Builders
{
    public class SickLeaveBuilder : ProjectBaseBuilder<SickLeaveBuilder>
    {
        private string? _medicalCertificateId;
        private bool _isPaid = true; // Default
        private double _missedHours;

        public SickLeaveBuilder WithMissedHours(double hours)
        {
            _missedHours = hours;
            return this;
        }

        public SickLeaveBuilder WithCertificate(string certificateId)
        {
            _medicalCertificateId = certificateId;
            return this;
        }

        public SickLeaveBuilder SetPaid(bool isPaid)
        {
            _isPaid = isPaid;
            return this;
        }

        public SickLeave Build()
        {
            return new SickLeave
            {
                Title = _title,           // Inherited from ProjectBaseBuilder
                StartDate = _startDate,   // Inherited from ProjectBaseBuilder
                EndDate = _endDate,       // Inherited from ProjectBaseBuilder
                MissedHours = _missedHours,
                MedicalCertificateId = _medicalCertificateId,
                IsPaid = _isPaid
            };
        }
    }
}