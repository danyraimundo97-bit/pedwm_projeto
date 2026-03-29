using System.Threading.Tasks;
using ApplicationLayer.Commands;
using ApplicationLayer.Services;
using ApplicationLayer.Strategy;
using DomainLayer.Domain.Notifications;
using DomainLayer.Domain.Users;

namespace ApplicationLayer.Handlers
{
    public class CreateUserHandler
    {
        private readonly IUserService _userService;
        private readonly NotificationSender _notificationSender;

        // Injetar (Serviço e Notificações)
        public CreateUserHandler(IUserService userService, NotificationSender notificationSender)
        {
            _userService = userService;
            _notificationSender = notificationSender;
        }

        public async Task<User> HandleAsync(CreateUserCommand command)
        {
            // Criar o user (O Serviço aplica as regras de negócio)
            var user = await _userService.CreateUserAsync(command);

            // Notificar o próprio utilizador que a sua conta foi criada
            var notif = new Notification
            {
                UserId = user.Id,
                Type = NotificationType.Info,
                Message = $"Olá {user.Name}, a tua conta foi criada com sucesso!"
            };

            await _notificationSender.DeliverAsync(user, notif);

            return user;
        }
    }
}