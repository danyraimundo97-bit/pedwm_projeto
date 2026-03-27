using DomainLayer.Domain.Projects;

namespace ApplicationLayer.Commands
{
    public class CreateProjectCommand
    {
        public ProjectType Type { get; set; } // "STANDARD" | "SICKLEAVE" | "HOLIDAY" | "TRAINING"
        public string Title { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double AllocatedHours { get; set; }
        public Guid ManagerId { get; set; }
        public Guid TeamId { get; set; }

        // Propriedades Opcionais (Específicas de cada tipo)

        // Standard
        public string? ClientName { get; set; }

        // SickLeave
        public string? MedicalCertificateId { get; set; }
        public bool? IsPaid { get; set; }

        // Holiday
        public string? HolidayType { get; set; } // "Fixed" ou "Optional"

        // Training
        public string? CourseName { get; set; }
        public string? CertificationLink { get; set; }
    }
}