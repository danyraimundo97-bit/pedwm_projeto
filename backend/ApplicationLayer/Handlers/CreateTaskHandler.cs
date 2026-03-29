using System.Threading.Tasks;
using ApplicationLayer.Commands;
using ApplicationLayer.Factories;
using ApplicationLayer.Mapping;
using ApplicationLayer.Models;
using ApplicationLayer.Services;
using ApplicationLayer.Strategy;
using DomainLayer.Ports;

namespace ApplicationLayer.Handlers
{
    public class CreateTaskHandler
    {
        private readonly ITaskRepository _repository;
        private readonly IDomainEntityDtoMapper _mapper;
        private readonly ITaskService _taskService;
        private readonly NotificationSender _notificationSender;

        // Injetar (Serviço e Notificações)
        public CreateTaskHandler(
            ITaskService taskService,
            ITaskRepository repository,
            IDomainEntityDtoMapper mapper,
            NotificationSender notificationSender)
        {
            _repository = repository;
            _mapper = mapper;
            _taskService = taskService;
            _notificationSender = notificationSender;
        }

        public async Task<TaskDto> HandleAsync(CreateTaskCommand command)
        {
            var task = TaskFromCommandFactory.Create(command);
            await _repository.SaveAsync(task);
            var dto = _mapper.ToTaskDto(task);

            var user = new UserDto
            {
                Id = Guid.NewGuid(),
                Name = "Developer",
                Email = string.Empty,
                Role = UserRole.Standard,
            };
            // Criar a tarefa (O Serviço aplica as regras de negócio)
            var task = await _taskService.CreateTaskAsync(command);

            var notif = new NotificationDto
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Type = NotificationType.Info,
                Message = $"Nova tarefa atribuída: '{dto.Title}'",
            };

            await _notificationSender.DeliverAsync(user, notif);

            return dto;
        }
    }
}
