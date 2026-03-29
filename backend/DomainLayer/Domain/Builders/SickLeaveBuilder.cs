using DomainLayer.Domain.Projects;

namespace DomainLayer.Domain.Builders
{
    public sealed class SickLeaveBuilder : ProjectBaseBuilder<SickLeaveBuilder>, IBuilder<SickLeave>
    {
        private string? _medicalCertificateId;
        private bool _isPaid = true;
        private double _missedHours;

        public SickLeaveBuilder WithMissedHours(double hours)
        {
            _missedHours = hours;
            return this;
        }

        public SickLeaveBuilder WithCertificate(string? certificateId)
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
            EnsureTitleAndDates();
            return new SickLeave(
                Guid.NewGuid(),
                _title.Trim(),
                _startDate,
                _endDate,
                _missedHours,
                _medicalCertificateId,
                _isPaid);
        }
    }
}
