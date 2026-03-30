using ApplicationLayer.Commands;
using ApplicationLayer.Models;
using ApplicationLayer.Services;
using ApplicationLayer.Strategy;
using DomainLayer.Domain.Notifications;
using DomainLayer.Domain.Teams;
using INotificationService = ApplicationLayer.Services.INotificationService;

namespace ApplicationLayer.Handlers
{
    public class CreateTeamHandler
    {
        private readonly ITeamService _teamService;
        private readonly INotificationService _notificationSender;

        // Injetar (Serviço e Notificações)
        public CreateTeamHandler(ITeamService teamService, INotificationService notificationSender)
        {
            _teamService = teamService;
            _notificationSender = notificationSender;
        }

        public async Task<Team> HandleAsync(CreateTeamCommand command)
        {
            // Criar a equipa (O Serviço aplica as regras de negócio)
            var team = await _teamService.CreateTeamAsync(command);

            // Simular notificação para o Admin do sistema
            var adminUser = new UserResponse { Name = "Admin do Sistema" };
            var notif = new Models.Notification
            {
                UserId = adminUser.Id,
                Type = NotificationType.Info,
                Message = $"Nova Equipa criada com sucesso: '{team.Name}'"
            };

            await _notificationSender.DeliverAsync(adminUser, notif);

            return team;
        }
    }
}