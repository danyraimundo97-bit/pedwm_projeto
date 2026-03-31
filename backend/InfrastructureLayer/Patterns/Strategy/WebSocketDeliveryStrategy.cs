using ApplicationLayer.Repositories;
using DomainLayer.Domain.Notifications;
using InfrastructureLayer.Patterns.Singleton;

namespace InfrastructureLayer.Patterns.Strategy
{
    public class WebSocketDeliveryStrategy : INotificationDeliveryStrategy
    {
        public Task SendAsync(Notification notification)
        {
            LoggerService.Instance.LogInfo($"[WEBSOCKET] Push real-time para utilizador {notification.UserId}: {notification.Message}");
            //TODO: Implementar o código real do SignalR para enviar a notificação em tempo real para o Flutter

            return Task.CompletedTask;
        }
    }
}
