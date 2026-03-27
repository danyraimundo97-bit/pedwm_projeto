using DomainLayer.Domain.Projects;

namespace DomainLayer.Builders
{
    public class HolidayBuilder : ProjectBaseBuilder<HolidayBuilder>
    {
        private HolidayType _type = HolidayType.Optional;

        public HolidayBuilder WhichType(HolidayType type)
        {
            _type = type;
            return this;
        }

        public Holiday Build()
        {
            return new Holiday
            {
                Title = _title,             // Inherited from ProjectBaseBuilder
                StartDate = _startDate,     // Inherited from ProjectBaseBuilder
                EndDate = _endDate,         // Inherited from ProjectBaseBuilder
                Type = _type,
                HolidayHours = (_endDate - _startDate).TotalDays * 8
            };
        }
    }
}