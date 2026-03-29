using DomainLayer.Domain.Projects;

namespace DomainLayer.Domain.Builders
{
    public sealed class HolidayBuilder : ProjectBaseBuilder<HolidayBuilder>, IBuilder<Holiday>
    {
        private HolidayType _type = HolidayType.Optional;

        public HolidayBuilder WhichType(HolidayType type)
        {
            _type = type;
            return this;
        }

        public Holiday Build()
        {
            EnsureTitleAndDates();
            var holidayHours = Math.Max(0, (_endDate - _startDate).TotalDays * 8);
            return new Holiday(
                Guid.NewGuid(),
                _title.Trim(),
                _startDate,
                _endDate,
                _type,
                holidayHours);
        }
    }
}
