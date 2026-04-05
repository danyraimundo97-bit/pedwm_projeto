using DomainLayer.Domain.Notifications;

namespace ApplicationLayer.Repositories
{
    /// <summary>Runs every registered delivery strategy (e.g. email log + SignalR push).</summary>
    public sealed class CompositeNotificationDeliveryStrategy : INotificationDeliveryStrategy
    {
        private readonly IReadOnlyList<INotificationDeliveryStrategy> _strategies;

        public CompositeNotificationDeliveryStrategy(IEnumerable<INotificationDeliveryStrategy> strategies)
        {
            _strategies = strategies.ToList();
        }

        public async Task SendAsync(Notification notification)
        {
            foreach (var s in _strategies)
            {
                await s.SendAsync(notification);
            }
        }
    }
}
