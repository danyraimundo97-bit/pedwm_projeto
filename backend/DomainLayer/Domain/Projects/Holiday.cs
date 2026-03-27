namespace DomainLayer.Domain.Projects
{
    public class Holiday : ProjectBase
    {
        public HolidayType Type { get; set; } = HolidayType.Optional;
        public double HolidayHours { get; set; }

        // Método abstrato obrigatório
        public override double GetTotalAllocatedHours()
        {
            return HolidayHours;
        }
    }
}