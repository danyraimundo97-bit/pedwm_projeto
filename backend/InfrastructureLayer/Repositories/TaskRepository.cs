using ApplicationLayer.Repositories;
using DomainLayer.Domain.Tasks;
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

        // Salva uma tarefa na base de dados de forma assíncrona
        public async Task SaveAsync(TaskBase task)
        {
            // TODO: Verificar se o projeto já existe e atualizar em vez de criar um novo, para evitar duplicados
            //TODO: Adicionar tratamento de erros (try-catch) para lidar com possíveis falhas na base de dados
            //TODO: Implementar logging mais detalhado (ex: sucesso, falha)
            //TODO: Verificar se o projeto é válido antes de tentar salvar (ex: campos obrigatórios)
            LoggerService.Instance.Log($"[DATABASE] A guardar a tarefa {task.Id} na BD...");

            _context.Tasks.Add(task); // Adiciona a tarefa ao DbSet
            await _context.SaveChangesAsync(); // Salva as alterações na base de dados
             //TODO: Verificar o resultado do SaveChangesAsync para confirmar que a operação foi bem-sucedida
            //TODO: Logar o resultado da operação
        }

        // Obter todas as tarefas da base de dados de forma assíncrona
        public async Task<IEnumerable<TaskBase>> GetAllAsync()
        {
            LoggerService.Instance.Log("[DATABASE] A ler todas as tarefas...");
            return await _context.Tasks.ToListAsync();
        }

        Task<IReadOnlyList<TaskBase>> ITaskRepository.GetAllAsync()
        {
            throw new NotImplementedException();
        }
    }
}
