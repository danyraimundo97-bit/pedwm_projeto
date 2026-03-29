using System.Threading.Tasks;
using DomainLayer.Domain.Users;
using DomainLayer.Domain.Notifications;

namespace ApplicationLayer.Strategy
{
    public class NotificationSender
    {
        private INotificationDeliveryStrategy _strategy;

        // O construtor exige uma estratégia inicial
        public NotificationSender(INotificationDeliveryStrategy strategy)
        {
            _strategy = strategy;
        }

        // Strategy: Permite mudar a forma de envio em tempo de execução
        public void SetStrategy(INotificationDeliveryStrategy strategy)
        {
            _strategy = strategy;
        }

        public async Task DeliverAsync(User user, Notification notif)
        {
            // Delegamos a responsabilidade de envio para a estratégia atual
            await _strategy.SendAsync(user, notif);
        }
    }
}