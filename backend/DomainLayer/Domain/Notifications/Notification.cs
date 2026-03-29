using DomainLayer.Domain.Builders;

namespace DomainLayer.Domain.Notifications
{
    /// <summary>Fluent construction via <see cref="NotificationBuilder"/>.</summary>
    public class Notification
    {
        public Guid Id { get; private set; }

        public Guid UserId { get; private set; }

        public NotificationType Type { get; private set; }

        public string Message { get; private set; } = string.Empty;

        private Notification()
        {
        }

        internal Notification(Guid id, Guid userId, NotificationType type, string message)
        {
            Id = id;
            UserId = userId;
            Type = type;
            Message = message;
        }

        public static NotificationBuilder Builder() => new NotificationBuilder();
    }
}
