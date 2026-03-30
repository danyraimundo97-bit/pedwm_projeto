using DomainLayer.Domain.Builders;

namespace DomainLayer.Domain.Projects
{
    public class    Holiday : ProjectBase
    {
        public HolidayType Type { get; private set; } = HolidayType.Optional;

        public double HolidayHours { get; private set; }

        private Holiday()
        {
        }

        internal Holiday(
            Guid id,
            string title,
            DateTime startDate,
            DateTime endDate,
            HolidayType type,
            double holidayHours)
            : base(id, title, startDate, endDate)
        {
            Type = type;
            HolidayHours = holidayHours;
        }

        public override double GetTotalAllocatedHours()
        {
            return HolidayHours;
        }

        public static HolidayBuilder Builder() => new HolidayBuilder();
    }
}
