using ApplicationLayer.Models;
using ApplicationLayer.Repositories;
using InfrastructureLayer.Patterns.Singleton;

namespace InfrastructureLayer.Patterns.Strategy
{
    public class EmailDeliveryStrategy : INotificationDeliveryStrategy
    {
        public Task SendAsync(UserResponse user, ApplicationLayer.Models.Notification notification)
        {
            LoggerService.Instance.Log($"[EMAIL] A enviar email para {user.Name}: {notification.Message}");
            LoggerService.Instance.Log($"[EMAIL] Email enviado com sucesso!!");

            return Task.CompletedTask;
        }
    }
}
