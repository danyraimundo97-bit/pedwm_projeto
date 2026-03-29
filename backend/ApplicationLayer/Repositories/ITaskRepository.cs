using System.Threading.Tasks;
using DomainLayer.Domain.Tasks;

namespace ApplicationLayer.Repositories
{
    public interface ITaskRepository
    {
        // Salvar uma Task
        Task SaveAsync(TaskBase task);

        // Obter todas as Tasks
        Task<IEnumerable<TaskBase>> GetAllAsync();
    }
}