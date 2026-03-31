using Microsoft.EntityFrameworkCore;
using DomainLayer.Domain.Tasks;
using ApplicationLayer.Repositories;
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

        // SAVE
        public async Task SaveAsync(TaskBase task)
        {
            LoggerService.Instance.LogInfo($"[DATABASE] A processar a Tarefa '{task.Title}' (ID: {task.Id})...");

            // Verificar se a tarefa já existe ('AsNoTracking' para performance)
            bool exists = await _context.Tasks.AsNoTracking().AnyAsync(t => t.Id == task.Id);

            if (exists)
            {
                _context.Tasks.Update(task);
                LoggerService.Instance.LogInfo($"[DATABASE] A atualizar a Tarefa {task.Id}.");
            }
            else
            {
                await _context.Tasks.AddAsync(task);
                LoggerService.Instance.LogInfo($"[DATABASE] A inserir a Tarefa {task.Id}.");
            }

            // Verificar o resultado
            int rowsAffected = await _context.SaveChangesAsync();
            LoggerService.Instance.LogInfo($"[DATABASE] Operação concluída com {rowsAffected} linhas afetadas");
        }

        // GET ALL
        public async Task<IReadOnlyList<TaskBase>> GetAllAsync()
        {
            LoggerService.Instance.LogInfo("[DATABASE] A iniciar a leitura de todas as tarefas...");

            var tasks = await _context.Tasks.ToListAsync();

            LoggerService.Instance.LogInfo($"[DATABASE] Leitura de {tasks.Count} tarefas concluída.");

            return tasks;
        }
    }
}