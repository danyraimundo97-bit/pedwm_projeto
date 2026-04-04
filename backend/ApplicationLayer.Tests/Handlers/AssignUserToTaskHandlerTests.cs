using System;
using System.Threading.Tasks;
using ApplicationLayer.Commands;
using ApplicationLayer.Handlers;
using ApplicationLayer.Services;
using DomainLayer.Domain.Tasks;
using DomainLayer.Domain.Notifications;
using DomainLayer.Domain.Builders;
using Moq;
using FluentAssertions;
using Xunit;

namespace ApplicationLayer.Tests.Handlers
{
    public class AssignUserToTaskHandlerTests
    {
        private readonly Mock<ISessionService> _sessionServiceMock;
        private readonly Mock<ITaskService> _taskServiceMock;
        private readonly Mock<INotificationService> _notificationSenderMock;
        private readonly AssignUserToTaskHandler _sut;

        public AssignUserToTaskHandlerTests()
        {
            _sessionServiceMock = new Mock<ISessionService>();
            _taskServiceMock = new Mock<ITaskService>();
            _notificationSenderMock = new Mock<INotificationService>();

            _sut = new AssignUserToTaskHandler(
                _sessionServiceMock.Object,
                _taskServiceMock.Object,
                _notificationSenderMock.Object);
        }

        [Fact]
        public async Task HandleAsync_ShouldSendNotification_WhenUserIsAssignedToTask()
        {
            var adminId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var taskId = Guid.NewGuid();
            var projectId = Guid.NewGuid();

            var command = new AssignTaskToUserCommand
            {
                AssigneeUserId = userId.ToString(),
                TaskId = taskId.ToString(),
                ProjectId = projectId.ToString()
            };

            // Criamos um fakeUser
            var fakeUser = new UserBuilder()
                  .WithId(userId)
                  .WithName("Carlos")
                  .Build();

            _sessionServiceMock.Setup(s => s.GetCurrentUserID()).Returns(adminId);

            _taskServiceMock.Setup(s => s.AssignUserToTaskAsync(
                command.AssigneeUserId, command.TaskId, command.ProjectId))
                .ReturnsAsync(fakeUser);

            await _sut.HandleAsync(command);

            // O Admin que fez a atribuição recebeu o aviso de sucesso
            _notificationSenderMock.Verify(n => n.DeliverAsync(It.Is<Notification>(notif =>
                notif.UserId == adminId &&
                notif.Message.Contains("atribuída")
            )), Times.Once);
        }
    }
}