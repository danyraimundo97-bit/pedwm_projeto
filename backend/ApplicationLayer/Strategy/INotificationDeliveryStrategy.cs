using System.Threading.Tasks;
using DomainLayer.Domain;

namespace ApplicationLayer.Strategy
{
    public interface INotificationDeliveryStrategy
    {
        Task SendAsync(User user, Notification notification);
    }
}