using DomainLayer.Domain.Tasks;

namespace ApplicationLayer.Repositories
{
    /// <summary>Outbound port: persistence for <see cref="TaskBase"/> (implemented in Infrastructure).</summary>
    public interface ITaskRepository
    {
        Task SaveAsync(TaskBase task);

        Task<IReadOnlyList<TaskBase>> GetAllAsync();

        Task<TaskBase?> GetTaskAsync(string taskId, string projectId);
    }
}
