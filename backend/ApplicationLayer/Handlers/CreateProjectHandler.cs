using ApplicationLayer.Commands;
using ApplicationLayer.Mapping;
using ApplicationLayer.Models;
using ApplicationLayer.Services;
using DomainLayer.Domain.Builders;
using DomainLayer.Domain.Notifications;
using DomainLayer.Domain.Users;
using System.Diagnostics.Tracing;

namespace ApplicationLayer.Handlers
{
    public class CreateProjectHandler
    {
        private readonly IProjectService _projectService;
        private readonly Mapper _mapper;
        private readonly INotificationService _notificationService;
        private readonly ISessionService _sessionService;

        // Injetar (Serviço e Notificações)
        public CreateProjectHandler(
            IProjectService projectService,
            Mapper mapper,
            INotificationService notificationService,
            ISessionService sessionService)
        {
            _mapper = mapper;
            _projectService = projectService;
            _notificationService = notificationService;
            _sessionService = sessionService;
        }

        public async Task<ProjectSender> HandleAsync(CreateProjectCommand command)
        {
            // Criar o projeto (O Serviço aplica as regras de negócio)
            var project = await _projectService.CreateProjectAsync(command);
            //var project = ProjectFromCommandFactory.Create(command);
            //await _repository.SaveAsync(project);
            var projectResponse = _mapper.ToProjectSender(project);
            Guid userId = _sessionService.GetCurrentUserID();


            var notif = new NotificationBuilder()
                .WithId(Guid.NewGuid())
                .WithType(NotificationType.Info)
                .ForUser(userId)
                .WithMessage($"O projeto '{projectResponse.Title}' foi criado e guardado com sucesso!")
                .Build();

            await _notificationService.DeliverAsync(notif);

            return projectResponse;
        }
    }
}
