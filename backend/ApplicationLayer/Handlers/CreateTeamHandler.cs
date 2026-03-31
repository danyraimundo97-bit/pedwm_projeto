using ApplicationLayer.Commands;
using ApplicationLayer.Services;
using DomainLayer.Domain.Builders;
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
            var adminUserId = Guid.NewGuid();
            var notif = new NotificationBuilder()
                .WithId(Guid.NewGuid())
                .ForUser(adminUserId)
                .WithType(NotificationType.Info)
                .WithMessage($"Nova Equipa criada com sucesso: '{team.Name}'")
                .Build();

            await _notificationSender.DeliverAsync(notif);

            return team;
        }
    }
}