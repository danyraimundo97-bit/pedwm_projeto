using System.Threading.Tasks;
using DomainLayer.Domain.Users;
using DomainLayer.Domain.Notifications;

namespace ApplicationLayer.Strategy
{
    public interface INotificationDeliveryStrategy
    {
        Task SendAsync(User user, Notification notification);
    }
}