using System;
using System.Threading.Tasks;
using ApplicationLayer.Commands;
using ApplicationLayer.Factories;
using ApplicationLayer.Repositories;
using ApplicationLayer.Strategy;
using DomainLayer.Domain;
using DomainLayer.Domain.Tasks;

namespace ApplicationLayer.Handlers
{
    public class CreateTaskHandler
    {
        private readonly ProjectTaskFactory _factory;
        private readonly ITaskRepository _repository;
        private readonly NotificationSender _notificationSender;

        // Injetar as dependências que o Handler precisa
        public CreateTaskHandler(
            ProjectTaskFactory factory,
            ITaskRepository repository,
            NotificationSender notificationSender)
        {
            _factory = factory;
            _repository = repository;
            _notificationSender = notificationSender;
        }

        public async Task<TaskBase> HandleAsync(CreateTaskCommand command)
        {
            // Criar a tarefa (Usa a Factory e o Builder)
            var task = _factory.CreateFromCommand(command);

            // Guardar na Base de Dados (Repository)
            await _repository.SaveAsync(task);

            // Preparar a notificação
            var user = new User { Id = Guid.NewGuid(), Name = "Developer" };
            var notif = new Notification
            {
                UserId = user.Id,
                Type = NotificationType.Info,
                Message = $"Nova tarefa atribuída: '{task.Title}'"
            };

            // Enviar a notificação (Strategy)
            await _notificationSender.DeliverAsync(user, notif);

            return task; // Devolve a tarefa criada
        }
    }
}