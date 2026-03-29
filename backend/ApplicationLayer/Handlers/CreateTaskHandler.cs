using ApplicationLayer.Commands;
using ApplicationLayer.Factories;
using ApplicationLayer.Mapping;
using ApplicationLayer.Models;
using ApplicationLayer.Strategy;
using DomainLayer.Ports;

namespace ApplicationLayer.Handlers
{
    public class CreateTaskHandler
    {
        private readonly ITaskRepository _repository;
        private readonly IDomainEntityDtoMapper _mapper;
        private readonly NotificationSender _notificationSender;

        public CreateTaskHandler(
            ITaskRepository repository,
            IDomainEntityDtoMapper mapper,
            NotificationSender notificationSender)
        {
            _repository = repository;
            _mapper = mapper;
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
