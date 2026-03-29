using ApplicationLayer.Models;

namespace PresentationLayer.DTOs
{
    public class CreateProject_DTO
    {
        public ProjectType Type { get; set; }

        public string Title { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public double AllocatedHours { get; set; }

        public Guid ManagerId { get; set; }

        public Guid TeamId { get; set; }

        public string? ClientName { get; set; }

        public string? MedicalCertificateId { get; set; }

        public bool? IsPaid { get; set; }

        public string? HolidayType { get; set; }

        public string? CourseName { get; set; }

        public string? CertificationLink { get; set; }
    }
}
