using System.Threading.Tasks;
using ApplicationLayer.Commands;
using ApplicationLayer.Services;
using ApplicationLayer.Strategy;
using DomainLayer.Domain.Users;
using DomainLayer.Domain.Notifications;
using DomainLayer.Domain.Projects;

namespace ApplicationLayer.Handlers
{
    public class CreateProjectHandler
    {
        private readonly IProjectService _projectService;
        private readonly NotificationSender _notificationSender;

        // Injetar (Serviço e Notificações)
        public CreateProjectHandler(
            IProjectService projectService,
            NotificationSender notificationSender)
        {
            _projectService = projectService;
            _notificationSender = notificationSender;
        }

        public async Task<ProjectBase> HandleAsync(CreateProjectCommand command)
        {
            // Criar o projeto (O Serviço aplica as regras de negócio)
            var project = await _projectService.CreateProjectAsync(command);


            // Simular notificação para o Gestor do projeto
            var user = new User { Id = command.ManagerId.GetValueOrDefault(), Name = "Gestor do Projeto" };
            var notif = new Notification
            {
                UserId = user.Id,
                Type = NotificationType.Info,
                Message = $"O projeto '{project.Title}' foi criado e guardado com sucesso!"
            };

            // Enviar a notificação (Strategy)
            await _notificationSender.DeliverAsync(user, notif);

            return project;
        }
    }
}