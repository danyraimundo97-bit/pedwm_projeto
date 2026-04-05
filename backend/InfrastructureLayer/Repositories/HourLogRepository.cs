using ApplicationLayer.Repositories;
using DomainLayer.Domain.Hours;
using InfrastructureLayer.Data;
using Microsoft.EntityFrameworkCore;

namespace InfrastructureLayer.Repositories
{
    public sealed class HourLogRepository : IHourLogRepository
    {
        private readonly AppDbContext _context;

        public HourLogRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(HourLog log)
        {
            await _context.HourLogs.AddAsync(log);
            await _context.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<HourLog>> GetInRangeAsync(DateTime fromUtcInclusive, DateTime toUtcInclusive)
        {
            return await _context.HourLogs
                .AsNoTracking()
                .Where(h => h.LoggedAtUtc >= fromUtcInclusive && h.LoggedAtUtc <= toUtcInclusive)
                .OrderBy(h => h.LoggedAtUtc)
                .ToListAsync();
        }
    }
}
