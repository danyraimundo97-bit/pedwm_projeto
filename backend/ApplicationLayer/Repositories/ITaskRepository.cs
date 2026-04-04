using DomainLayer.Domain.Tasks;

namespace ApplicationLayer.Repositories
{
    /// <summary>Outbound port: persistence for <see cref="TaskBase"/> (implemented in Infrastructure).</summary>
    public interface ITaskRepository
    {
        Task SaveAsync(TaskBase task);
        Task<TaskBase?> GetByIdAsync(Guid id);
        Task<TaskBase?> GetTaskAsync(string taskId, string projectId);
        Task<IReadOnlyList<TaskBase>> GetPagedAsync(int page, int size);
        Task<IReadOnlyList<TaskBase>> GetByProjectAsync(Guid projectId);
        Task<IReadOnlyList<TaskBase>> GetByUserAsync(Guid userId);
    }
}
