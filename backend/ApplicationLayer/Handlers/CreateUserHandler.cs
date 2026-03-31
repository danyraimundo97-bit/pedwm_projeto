using System.Threading.Tasks;
using ApplicationLayer.Commands;
using ApplicationLayer.Services;
using DomainLayer.Domain.Builders;
using DomainLayer.Domain.Notifications;
using DomainLayer.Domain.Users;

namespace ApplicationLayer.Handlers
{
    public class CreateUserHandler
    {
        private readonly IUserService _userService;
        private readonly INotificationService _notificationService;

        // Injetar (Serviço e Notificações)
        public CreateUserHandler(IUserService userService, INotificationService notificationService)
        {
            _userService = userService;
            _notificationService = notificationService;
        }

        public async Task<User> HandleAsync(CreateUserCommand command)
        {
                // Criar o user (O Serviço aplica as regras de negócio)
                var user = await _userService.CreateUserAsync(command);

                // Notificar o próprio utilizador que a sua conta foi criada
                var notif = new NotificationBuilder()
                    .WithId(Guid.NewGuid())
                    .ForUser(user.Id)
                    .WithMessage($"Olá {user.Name}, a tua conta foi criada com sucesso!")
                    .WithType(NotificationType.Info)
                    .Build();

                await _notificationService.DeliverAsync(notif);

                return user;
        }
    }
}