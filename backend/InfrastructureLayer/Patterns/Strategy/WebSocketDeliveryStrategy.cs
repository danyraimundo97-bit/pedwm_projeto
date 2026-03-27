using System.Threading.Tasks;
using ApplicationLayer.Strategy;
using DomainLayer.Domain;
using InfrastructureLayer.Patterns.Singleton;

namespace InfrastructureLayer.Patterns.Strategy
{
    public class WebSocketDeliveryStrategy : INotificationDeliveryStrategy
    {
        public Task SendAsync(User user, Notification notification)
        {
            // Aqui estaria o código do SignalR para o Flutter
            LoggerService.Instance.Log($"[WEBSOCKET] Push real-time para ecrã de {user.Name}: {notification.Message}");
            //TODO: Implementar o código real do SignalR para enviar a notificação em tempo real para o Flutter

            return Task.CompletedTask;
        }
    }
}