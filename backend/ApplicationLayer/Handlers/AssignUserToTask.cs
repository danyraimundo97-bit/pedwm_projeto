using ApplicationLayer.Commands;
using ApplicationLayer.Services;
using DomainLayer.Domain.Builders;
using DomainLayer.Domain.Notifications;
using DomainLayer.Domain.Teams;
using DomainLayer.Domain.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationLayer.Handlers
{
    public class AssignUserToTaskHandler
    {
        private readonly ISessionService _sessionService;
        private readonly ITaskService _taskService;
        private readonly INotificationService _notificationSender;

        // Injetar (Serviço e Notificações)
        public AssignUserToTaskHandler(ISessionService sessionService,ITaskService taskService, INotificationService notificationSender)
        {
            _sessionService = sessionService;
            _taskService = taskService;
            _notificationSender = notificationSender;
        }

        public async void HandleAsync(AssignTaskToUserCommand command)
        {
            // Criar a equipa (O Serviço aplica as regras de negócio)
            var user = await _taskService.AssignUser(command.AssigneeUserId, command.TaskId, command.ProjectId);

            // Simular notificação para o Admin do sistema
            var adminUserId = _sessionService.GetCurrentUserID();
            var notif = new NotificationBuilder()
                .WithId(Guid.NewGuid())
                .ForUser(adminUserId)
                .WithType(NotificationType.Info)
                .WithMessage($"Tarefa foi associada ao utilizador com sucesso !")
                .Build();

            await _notificationSender.DeliverAsync(notif);
        }
    }
}
