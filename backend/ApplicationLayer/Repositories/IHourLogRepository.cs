using DomainLayer.Domain.Hours;

namespace ApplicationLayer.Repositories
{
    public interface IHourLogRepository
    {
        Task AddAsync(HourLog log);

        Task<IReadOnlyList<HourLog>> GetInRangeAsync(DateTime fromUtcInclusive, DateTime toUtcInclusive);
    }
}
