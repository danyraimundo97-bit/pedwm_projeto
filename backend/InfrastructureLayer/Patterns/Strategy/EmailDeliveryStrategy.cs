using ApplicationLayer.Models;
using ApplicationLayer.Strategy;
using InfrastructureLayer.Patterns.Singleton;

namespace InfrastructureLayer.Patterns.Strategy
{
    public class EmailDeliveryStrategy : INotificationDeliveryStrategy
    {
        public Task SendAsync(UserDto user, NotificationDto notification)
        {
            LoggerService.Instance.Log($"[EMAIL] A enviar email para {user.Name}: {notification.Message}");
            LoggerService.Instance.Log($"[EMAIL] Email enviado com sucesso!!");

            return Task.CompletedTask;
        }
    }
}
