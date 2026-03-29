using System.Threading.Tasks;
using ApplicationLayer.Commands;
using ApplicationLayer.Factories;
using ApplicationLayer.Mapping;
using ApplicationLayer.Models;
using ApplicationLayer.Strategy;
using DomainLayer.Ports;

namespace ApplicationLayer.Handlers
{
    public class CreateProjectHandler
    {
        private readonly IProjectService _projectService;
        private readonly IProjectRepository _repository;
        private readonly IDomainEntityDtoMapper _mapper;
        private readonly NotificationSender _notificationSender;

        // Injetar (Serviço e Notificações)
        public CreateProjectHandler(
            IProjectService projectService,
            IProjectRepository repository,
            IDomainEntityDtoMapper mapper,
            NotificationSender notificationSender)
        {
            _repository = repository;
            _mapper = mapper;
            _projectService = projectService;
            _notificationSender = notificationSender;
        }

        public async Task<ProjectDto> HandleAsync(CreateProjectCommand command)
        {
            // Criar o projeto (O Serviço aplica as regras de negócio)
            var project = await _projectService.CreateProjectAsync(command);
            var project = ProjectFromCommandFactory.Create(command);
            await _repository.SaveAsync(project);
            var dto = _mapper.ToProjectDto(project);

            var user = new UserDto
            {
                Id = command.ManagerId,
                Name = "Gestor do Projeto",
                Email = string.Empty,
                Role = UserRole.Standard,
            };

            var notif = new NotificationDto
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
