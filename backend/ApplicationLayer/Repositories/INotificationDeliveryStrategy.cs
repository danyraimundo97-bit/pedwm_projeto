using ApplicationLayer.Models;

namespace ApplicationLayer.Repositories
{
    public interface INotificationDeliveryStrategy
    {
        Task SendAsync(UserSender user, Models.NotificationSender notification);
    }
}
