using System.Threading.Tasks;
using ApplicationLayer.Commands;
using ApplicationLayer.Mapping;
using ApplicationLayer.Models;
using ApplicationLayer.Services;
using DomainLayer.Domain.Builders;
using DomainLayer.Domain.Notifications;
using DomainLayer.Domain.Users;

namespace ApplicationLayer.Handlers
{
    public class CreateTaskHandler
    {
        private readonly Mapper _mapper;
        private readonly ITaskService _taskService;
        private readonly INotificationService _notificationService;

        // Injetar (Serviço e Notificações)
        public CreateTaskHandler(
            ITaskService taskService,
            Mapper mapper,
            INotificationService notificationService)
        {
            _mapper = mapper;
            _taskService = taskService;
            _notificationService = notificationService;
        }

        public async Task<TaskResponse> HandleAsync(CreateTaskCommand command)
        {
            // Criar a tarefa (O Serviço aplica as regras de negócio)
            var task = await _taskService.CreateTaskAsync(command);
            //var task = TaskFromCommandFactory.Create(command);
            var dto = _mapper.ToTaskResponse(task);

            var user = new UserResponse
            {
                Id = Guid.NewGuid(),
                Name = "Developer",
                Email = string.Empty,
                Role = UserRole.Standard,
            };


            var notif = new NotificationBuilder()
                .WithId(Guid.NewGuid())
                .ForUser(user.Id)
                .WithType(NotificationType.Info)
                .WithMessage($"Nova tarefa atribuída: '{dto.Title}'")
                .Build();

            await _notificationService.DeliverAsync(notif);

            return dto;
        }
    }
}
