using System.Threading.Tasks;
using ApplicationLayer.Strategy;
using DomainLayer.Domain;
using InfrastructureLayer.Patterns.Singleton;

namespace InfrastructureLayer.Patterns.Strategy
{
    public class EmailDeliveryStrategy : INotificationDeliveryStrategy
    {
        public Task SendAsync(User user, Notification notification)
        {
            // Aqui estaria o código do SMTP ou SendGrid
            LoggerService.Instance.Log($"[EMAIL] A enviar email para {user.Name}: {notification.Message}");
            return Task.CompletedTask;
        }
    }
}