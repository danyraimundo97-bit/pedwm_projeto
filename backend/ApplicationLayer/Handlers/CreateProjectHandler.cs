using ApplicationLayer.Commands;
using ApplicationLayer.Factories;
using ApplicationLayer.Repositories;
using ApplicationLayer.Strategy;
using DomainLayer.Domain;
using DomainLayer.Domain.Projects;
using System.Threading.Tasks;

namespace ApplicationLayer.Handlers
{
    public class CreateProjectHandler
    {
        private readonly ProjectFactory _factory;
        private readonly IProjectRepository _repository;
        private readonly NotificationSender _notificationSender;

        // Injetar as dependências que o Handler precisa
        public CreateProjectHandler(
            ProjectFactory factory,
            IProjectRepository repository,
            NotificationSender notificationSender)
        {
            _factory = factory;
            _repository = repository;
            _notificationSender = notificationSender;
        }

        public async Task<ProjectBase> HandleAsync(CreateProjectCommand command)
        {
            // Criar o projeto (Usa a Factory e o Builder)
            var project = _factory.CreateFromCommand(command);

            // Guardar na Base de Dados (Repository)
            await _repository.SaveAsync(project);

            // Preparar a notificação
            var user = new User { Id = command.ManagerId, Name = "Gestor do Projeto" };
            var notif = new Notification
            {
                UserId = user.Id,
                Type = NotificationType.Info,
                Message = $"O projeto '{project.Title}' foi criado e guardado com sucesso!"
            };

            // Enviar a notificação (Strategy)
            await _notificationSender.DeliverAsync(user, notif);

            return project; // Devolve o projeto criado
        }
    }
}