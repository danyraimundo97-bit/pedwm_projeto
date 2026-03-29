using System.Threading.Tasks;
using ApplicationLayer.Commands;
using ApplicationLayer.Services;
using ApplicationLayer.Strategy;
using DomainLayer.Domain.Users;
using DomainLayer.Domain.Notifications;
using DomainLayer.Domain.Tasks;

namespace ApplicationLayer.Handlers
{
    public class CreateTaskHandler
    {
        private readonly ITaskService _taskService;
        private readonly NotificationSender _notificationSender;

        // Injetar (Serviço e Notificações)
        public CreateTaskHandler(
            ITaskService taskService,
            NotificationSender notificationSender)
        {
            _taskService = taskService;
            _notificationSender = notificationSender;
        }

        public async Task<TaskBase> HandleAsync(CreateTaskCommand command)
        {
            // Criar a tarefa (O Serviço aplica as regras de negócio)
            var task = await _taskService.CreateTaskAsync(command);

            // Simular notificação para o Gestor do projeto
            var user = new User { Id = command.ProjectId, Name = "Gestor do Projeto" };
            var notif = new Notification
            {
                UserId = user.Id,
                Type = NotificationType.Info,
                Message = $"Nova tarefa criada: '{task.Title}'"
            };

            // Enviar a notificação (Strategy)
            await _notificationSender.DeliverAsync(user, notif);

            return task;
        }
    }
}