using System;
using System.Threading.Tasks;
using ApplicationLayer.Commands;
using ApplicationLayer.Services;
using DomainLayer.Domain.Projects;
using DomainLayer.Domain.Builders;
using DomainLayer.Domain.Notifications;

namespace ApplicationLayer.Handlers
{
    // Injetar (Serviço e Notificações)
    public class ChangeProjectStatusHandler
    {
        private readonly ISessionService _sessionService;
        private readonly IProjectService _projectService;
        private readonly INotificationService _notificationSender;

        public ChangeProjectStatusHandler(ISessionService sessionService, IProjectService projectService, INotificationService notificationSender)
        {
            _sessionService = sessionService;
            _projectService = projectService;
            _notificationSender = notificationSender;
        }

        public async Task<ProjectBase> HandleAsync(ChangeProjectStatusCommand command)
        {
            // O serviço trata da lógica e validações de negócio
            var project = await _projectService.ChangeProjectStatusAsync(command);

            var adminUserId = _sessionService.GetCurrentUserID();
            var notif = new NotificationBuilder()
                .WithId(Guid.NewGuid())
                .ForUser(adminUserId)
                .WithType(NotificationType.Info)
                .WithMessage($"O estado do projeto '{project.Title}' foi alterado para {command.Status}.")
                .Build();

            await _notificationSender.DeliverAsync(notif);

            return project;
        }
    }
}