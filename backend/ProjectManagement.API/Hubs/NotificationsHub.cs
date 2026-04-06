using Microsoft.AspNetCore.SignalR;

namespace PresentationLayer.Hubs
{
    /// <summary>Real-time notification channel; clients call <see cref="JoinUserNotifications"/> after login.</summary>
    public sealed class NotificationsHub : Hub
    {
        public Task JoinUserNotifications(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Task.CompletedTask;
            }

            return Groups.AddToGroupAsync(Context.ConnectionId, $"user-{userId}");
        }
    }
}
