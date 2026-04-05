using System;
using System.Threading.Tasks;
using ApplicationLayer.Commands;
using ApplicationLayer.Services;
using DomainLayer.Domain.Projects;
using DomainLayer.Domain.Builders;
using DomainLayer.Domain.Notifications;

namespace ApplicationLayer.Handlers
{
    public class AddHoursToProjectHandler
    {
        private readonly ISessionService _sessionService;
        private readonly IProjectService _projectService;
        private readonly INotificationService _notificationSender;

        // Injetar (Serviço e Notificações)
        public AddHoursToProjectHandler(ISessionService sessionService, IProjectService projectService, INotificationService notificationSender)
        {
            _sessionService = sessionService;
            _projectService = projectService;
            _notificationSender = notificationSender;
        }

        public async Task<ProjectBase> HandleAsync(AddHoursToProjectCommand command)
        {
            command.UserId = _sessionService.GetCurrentUserID();

            // O serviço trata da lógica e validações de negócio
            var project = await _projectService.AddConsumedHoursToProjectAsync(command);

            var adminUserId = _sessionService.GetCurrentUserID();
            var notif = new NotificationBuilder()
                .WithId(Guid.NewGuid())
                .ForUser(adminUserId)
                .WithType(NotificationType.Info)
                .WithMessage($"Foram registadas {command.Hours} horas no projeto '{project.Title}'.")
                .Build();

            await _notificationSender.DeliverAsync(notif);

            return project;
        }
    }
}