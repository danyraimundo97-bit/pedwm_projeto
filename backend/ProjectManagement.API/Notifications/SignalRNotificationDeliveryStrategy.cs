using ApplicationLayer.Repositories;
using DomainLayer.Domain.Notifications;
using Microsoft.AspNetCore.SignalR;
using PresentationLayer.Hubs;

namespace PresentationLayer.Notifications
{
    public sealed class SignalRNotificationDeliveryStrategy : INotificationDeliveryStrategy
    {
        private readonly IHubContext<NotificationsHub> _hubContext;

        public SignalRNotificationDeliveryStrategy(IHubContext<NotificationsHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendAsync(Notification notification)
        {
            var groupName = $"user-{notification.UserId}";
            await _hubContext.Clients.Group(groupName).SendAsync(
                "notification",
                new
                {
                    notification.Id,
                    notification.UserId,
                    Type = (int)notification.Type,
                    notification.Message,
                });
        }
    }
}
