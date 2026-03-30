using ApplicationLayer.Commands;
using ApplicationLayer.Mapping;
using ApplicationLayer.Models;
using ApplicationLayer.Services;
using ApplicationLayer.Strategy;

namespace ApplicationLayer.Handlers
{
    public class CreateProjectHandler
    {
        private readonly IProjectService _projectService;
        private readonly IDomainEntityDtoMapper _mapper;
        private readonly Strategy.NotificationSender _notificationSender;

        // Injetar (Serviço e Notificações)
        public CreateProjectHandler(
            IProjectService projectService,
            IDomainEntityDtoMapper mapper,
            Strategy.NotificationSender notificationSender)
        {
            _mapper = mapper;
            _projectService = projectService;
            _notificationSender = notificationSender;
        }

        public async Task<ProjectSender> HandleAsync(CreateProjectCommand command)
        {
            // Criar o projeto (O Serviço aplica as regras de negócio)
            var project = await _projectService.CreateProjectAsync(command);
            //var project = ProjectFromCommandFactory.Create(command);
            //await _repository.SaveAsync(project);
            var dto = _mapper.ToProjectDto(project);

            var user = new UserSender
            {
                Id = (Guid)command.ManagerId, //TODO:Rever
                Name = "Gestor do Projeto",
                Email = string.Empty,
                Role = UserRole.Standard,
            };

            var notif = new Models.NotificationSender
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Type = NotificationType.Info,
                Message = $"O projeto '{dto.Title}' foi criado e guardado com sucesso!",
            };

            await _notificationSender.DeliverAsync(user, notif);

            return dto;
        }
    }
}
