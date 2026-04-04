using System;
using System.Threading.Tasks;
using ApplicationLayer.Commands;
using ApplicationLayer.Services;
using DomainLayer.Domain.Builders;
using DomainLayer.Domain.Notifications;
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

        public async Task HandleAsync(AssignTaskToUserCommand command)
        {
            var user = await _taskService.AssignUserToTaskAsync(command.AssigneeUserId, command.TaskId, command.ProjectId);

            var adminUserId = _sessionService.GetCurrentUserID();
            var notif = new NotificationBuilder()
                .WithId(Guid.NewGuid())
                .ForUser(adminUserId)
                .WithType(NotificationType.Info)
                .WithMessage($"Tarefa atribuída a {user.Name}.")
                .Build();

            await _notificationSender.DeliverAsync(notif);
        }
    }
}
