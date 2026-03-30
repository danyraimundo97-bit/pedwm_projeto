using ApplicationLayer.Models;

namespace ApplicationLayer.Strategy
{
    public interface INotificationDeliveryStrategy
    {
        Task SendAsync(UserSender user, Models.NotificationSender notification);
    }
}
