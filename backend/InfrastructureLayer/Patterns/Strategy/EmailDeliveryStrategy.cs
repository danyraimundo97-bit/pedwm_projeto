using ApplicationLayer.Repositories;
using DomainLayer.Domain.Notifications;
using InfrastructureLayer.Patterns.Singleton;

namespace InfrastructureLayer.Patterns.Strategy
{
    public class EmailDeliveryStrategy : INotificationDeliveryStrategy
    {
        public Task SendAsync(Notification notification)
        {
            LoggerService.Instance.LogInfo($"[EMAIL] A enviar email para utilizador {notification.UserId}: {notification.Message}");
            LoggerService.Instance.LogInfo($"[EMAIL] Email enviado com sucesso!!");

            return Task.CompletedTask;
        }
    }
}
