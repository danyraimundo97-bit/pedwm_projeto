using ApplicationLayer.Models;

namespace ApplicationLayer.Strategy
{
    public interface INotificationDeliveryStrategy
    {
        Task SendAsync(UserDto user, NotificationDto notification);
    }
}
