using DomainLayer.Domain.Tasks;

namespace DomainLayer.Domain.Repositories
{
    /// <summary>Outbound port: persistence for <see cref="TaskBase"/> (implemented in Infrastructure).</summary>
    public interface ITaskRepository
    {
        Task SaveAsync(TaskBase task);
    }
}
