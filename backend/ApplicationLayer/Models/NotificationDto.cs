namespace ApplicationLayer.Models
{
    public sealed class NotificationDto
    {
        public Guid Id { get; init; }

        public Guid UserId { get; init; }

        public NotificationType Type { get; init; }

        public string Message { get; init; } = string.Empty;
    }
}
