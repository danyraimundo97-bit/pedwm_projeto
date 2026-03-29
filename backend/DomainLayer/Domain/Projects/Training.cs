namespace DomainLayer.Domain.Projects
{
    public class Training : ProjectBase
    {
        public string CourseName { get; set; } = string.Empty;
        public string CertificationLink { get; set; } = string.Empty;
        public double TrainingHours { get; set; }

        // Método abstrato obrigatório
        public override double GetTotalAllocatedHours()
        {
            return TrainingHours;
        }
    }   
}