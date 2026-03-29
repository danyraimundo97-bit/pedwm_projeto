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
            //TODO: Implementar o código real do SignalR para enviar a notificação em tempo real para o Flutter

            return Task.CompletedTask;
        }
    }
}
