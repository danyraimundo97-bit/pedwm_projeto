using DomainLayer.Domain;
using DomainLayer.Domain.Notifications;

namespace DomainLayer.Domain.Builders
{
    public sealed class NotificationBuilder : IBuilder<Notification>
    {
        private Guid _id = Guid.NewGuid();
        private Guid _userId;
        private NotificationType _type = NotificationType.Info;
        private string _message = string.Empty;

        public NotificationBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }

        public NotificationBuilder ForUser(Guid userId)
        {
            _userId = userId;
            return this;
        }

        public NotificationBuilder WithType(NotificationType type)
        {
            _type = type;
            return this;
        }

        public NotificationBuilder WithMessage(string message)
        {
            _message = message;
            return this;
        }

        public Notification Build()
        {
            if (_userId == Guid.Empty)
            {
                throw new InvalidOperationException("Call ForUser(userId) before Build().");
            }

            if (string.IsNullOrWhiteSpace(_message))
            {
                throw new InvalidOperationException("Call WithMessage(...) before Build().");
            }

            return new Notification(_id, _userId, _type, _message.Trim());
        }
    }
}
