using ApplicationLayer.Models;
using ApplicationLayer.Repositories;
using DomainLayer.Domain.Notifications;

namespace ApplicationLayer.Services
{
    public class NotificationService : INotificationService
    {
        private INotificationDeliveryStrategy _strategy;

        // O construtor exige uma estratégia inicial
        public NotificationService(INotificationDeliveryStrategy strategy)
        {
            _strategy = strategy;
        }

        // Strategy: Permite mudar a forma de envio em tempo de execução
        public void SetStrategy(INotificationDeliveryStrategy strategy)
        {
            _strategy = strategy;
        }

        public async Task DeliverAsync(Notification notification)
        {
            // Delegamos a responsabilidade de envio para a estratégia atual
            await _strategy.SendAsync(notification);
        }
    }
}
