using ApplicationLayer.Models;
using ApplicationLayer.Strategy;
using InfrastructureLayer.Patterns.Singleton;

namespace InfrastructureLayer.Patterns.Strategy
{
    public class WebSocketDeliveryStrategy : INotificationDeliveryStrategy
    {
        public Task SendAsync(UserDto user, NotificationDto notification)
        {
            LoggerService.Instance.Log($"[WEBSOCKET] Push real-time para ecrã de {user.Name}: {notification.Message}");
            return Task.CompletedTask;
        }
    }
}
