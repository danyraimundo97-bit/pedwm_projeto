using System;
using System.Threading.Tasks;
using ApplicationLayer.Commands;
using ApplicationLayer.Services;
using DomainLayer.Domain.Users;
using DomainLayer.Domain.Builders;
using DomainLayer.Domain.Notifications;

namespace ApplicationLayer.Handlers
{
    public class AssignUserToTeamHandler
    {
        private readonly ISessionService _sessionService;
        private readonly IUserService _userService;
        private readonly INotificationService _notificationSender;

        // Injetar (Serviço e Notificações)
        public AssignUserToTeamHandler(ISessionService sessionService, IUserService userService, INotificationService notificationSender)
        {
            _sessionService = sessionService;
            _userService = userService;
            _notificationSender = notificationSender;
        }

        public async Task<User> HandleAsync(AssignUserToTeamCommand command)
        {
            // O serviço trata da lógica e validações de negócio
            var user = await _userService.AssignUserToTeamAsync(command);

            var adminUserId = _sessionService.GetCurrentUserID();
            var notif = new NotificationBuilder()
                .WithId(Guid.NewGuid())
                .ForUser(adminUserId)
                .WithType(NotificationType.Info)
                .WithMessage($"O utilizador {user.Name} foi adicionado à equipa com sucesso.")
                .Build();

            await _notificationSender.DeliverAsync(notif);

            return user;
        }
    }
}