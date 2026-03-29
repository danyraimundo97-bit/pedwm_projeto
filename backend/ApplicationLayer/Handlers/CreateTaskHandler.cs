using System.Threading.Tasks;
using ApplicationLayer.Commands;
using ApplicationLayer.Factories;
using ApplicationLayer.Mapping;
using ApplicationLayer.Models;
using ApplicationLayer.Services;
using ApplicationLayer.Strategy;

namespace ApplicationLayer.Handlers
{
    public class CreateTaskHandler
    {
        private readonly IDomainEntityDtoMapper _mapper;
        private readonly ITaskService _taskService;
        private readonly NotificationSender _notificationSender;

        // Injetar (Serviço e Notificações)
        public CreateTaskHandler(
            ITaskService taskService,
            IDomainEntityDtoMapper mapper,
            NotificationSender notificationSender)
        {
            _mapper = mapper;
            _taskService = taskService;
            _notificationSender = notificationSender;
        }

        public async Task<TaskDto> HandleAsync(CreateTaskCommand command)
        {
            // Criar a tarefa (O Serviço aplica as regras de negócio)
            var task = await _taskService.CreateTaskAsync(command);
            //var task = TaskFromCommandFactory.Create(command);
            var dto = _mapper.ToTaskDto(task);

            var user = new UserDto
            {
                Id = Guid.NewGuid(),
                Name = "Developer",
                Email = string.Empty,
                Role = UserRole.Standard,
            };


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
