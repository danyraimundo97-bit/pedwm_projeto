using DomainLayer.Domain.Tasks;
using DomainLayer.Ports;
using InfrastructureLayer.Data;
using InfrastructureLayer.Patterns.Singleton;

namespace InfrastructureLayer.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly AppDbContext _context;

        public TaskRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task SaveAsync(TaskBase task)
        {
            LoggerService.Instance.Log($"[DATABASE] A guardar a tarefa {task.Id} na BD...");

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
        }
    }
}
