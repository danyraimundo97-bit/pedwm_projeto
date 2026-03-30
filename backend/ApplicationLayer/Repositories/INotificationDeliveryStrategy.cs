using ApplicationLayer.Models;
using DomainLayer.Domain.Notifications;

namespace ApplicationLayer.Repositories
{
    public interface INotificationDeliveryStrategy
    {
        Task SendAsync(Notification notification);
    }
}
