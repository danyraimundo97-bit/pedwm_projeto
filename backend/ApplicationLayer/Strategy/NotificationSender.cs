using ApplicationLayer.Models;

namespace ApplicationLayer.Strategy
{
    public class NotificationSender
    {
        private INotificationDeliveryStrategy _strategy;

        public NotificationSender(INotificationDeliveryStrategy strategy)
        {
            _strategy = strategy;
        }

        public void SetStrategy(INotificationDeliveryStrategy strategy)
        {
            _strategy = strategy;
        }

        public async Task DeliverAsync(UserDto user, NotificationDto notification)
        {
            await _strategy.SendAsync(user, notification);
        }
    }
}
