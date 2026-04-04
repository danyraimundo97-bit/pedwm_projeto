using System.Threading.Tasks;
using ApplicationLayer.Commands;
using DomainLayer.Domain.Tasks;
using DomainLayer.Domain.Users;

namespace ApplicationLayer.Services
{
    // Interface: Contrato que define o que faz o TaskService
    public interface ITaskService
    {
        Task<User> AssignUserToTaskAsync(string assigneeUserId, string taskId, string projectId);

        Task<TaskBase> CreateTaskAsync(CreateTaskCommand command);
    }
}