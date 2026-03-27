namespace DomainLayer.Domain.Projects
{
    public class SickLeave : ProjectBase
    {
        // O ID do atestado médico (pode ser nulo)
        public string? MedicalCertificateId { get; set; }
        public bool IsPaid { get; set; }

        public double MissedHours { get; set; }

        // Método abstrato obrigatório
        public override double GetTotalAllocatedHours()
        {
            return MissedHours;
        }
    }
}