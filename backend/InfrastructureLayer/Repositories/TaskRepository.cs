using System.Threading.Tasks;
using ApplicationLayer.Repositories;
using DomainLayer.Domain;
using DomainLayer.Domain.Tasks;
using InfrastructureLayer.Data;
using InfrastructureLayer.Patterns.Singleton;

namespace InfrastructureLayer.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly AppDbContext _context;

        // Injeção da dependência do DbContext para aceder à base de dados
        public TaskRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task SaveAsync(TaskBase task)
        {
            LoggerService.Instance.Log($"[DATABASE] A guardar a tarefa {task.Id} na BD...");

            _context.Tasks.Add(task); // Adiciona a tarefa ao DbSet
            await _context.SaveChangesAsync(); // Salva as alterações na base de dados
        }
    }
}