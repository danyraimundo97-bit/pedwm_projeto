using System.Threading.Tasks;
using ApplicationLayer.Commands;
using DomainLayer.Domain.Tasks;

namespace ApplicationLayer.Services
{
    // Interface: Contrato que define o que faz o TaskService
    public interface ITaskService
    {
        Task<TaskBase> CreateTaskAsync(CreateTaskCommand command);
    }
}