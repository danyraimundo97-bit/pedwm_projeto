using System.Threading.Tasks;
using DomainLayer.Domain.Tasks;

namespace ApplicationLayer.Repositories
{
    public interface ITaskRepository
    {
        Task SaveAsync(TaskBase task);
    }
}