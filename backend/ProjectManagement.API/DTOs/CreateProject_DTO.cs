using ApplicationLayer.Models;
using DomainLayer.Domain.Projects;

namespace PresentationLayer.DTOs
{
    public class CreateProject_DTO
    {
        public ProjectType Type { get; set; }

        public string Title { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public double AllocatedHours { get; set; }

        public string? ClientName { get; set; }
        public Guid? ManagerId { get; set; }
        public Guid? TeamId { get; set; }

        public string? MedicalCertificateId { get; set; }

        public bool? IsPaid { get; set; }

        // Holiday
        public HolidayType? HolidayType { get; set; } // "Fixed" ou "Optional"

        public string? CourseName { get; set; }

        public string? CertificationLink { get; set; }
    }
}
