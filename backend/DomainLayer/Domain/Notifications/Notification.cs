using System;

namespace DomainLayer.Domain
{
    public class Notification
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public NotificationType Type { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}