using System;
using System.Threading.Tasks;
using ApplicationLayer.Commands;
using ApplicationLayer.Handlers;
using ApplicationLayer.Services;
using DomainLayer.Domain.Projects;
using DomainLayer.Domain.Notifications;
using DomainLayer.Domain.Builders;
using Moq;
using FluentAssertions;
using Xunit;

namespace ApplicationLayer.Tests.Handlers
{
    public class ChangeProjectStatusHandlerTests
    {
        private readonly Mock<ISessionService> _sessionServiceMock;
        private readonly Mock<IProjectService> _projectServiceMock;
        private readonly Mock<INotificationService> _notificationSenderMock;
        private readonly ChangeProjectStatusHandler _sut;

        public ChangeProjectStatusHandlerTests()
        {
            _sessionServiceMock = new Mock<ISessionService>();
            _projectServiceMock = new Mock<IProjectService>();
            _notificationSenderMock = new Mock<INotificationService>();

            _sut = new ChangeProjectStatusHandler(
                _sessionServiceMock.Object,
                _projectServiceMock.Object,
                _notificationSenderMock.Object);
        }

        [Fact]
        public async Task HandleAsync_ShouldSendNotification_WhenStatusIsChangedSuccessfully()
        {
            var adminId = Guid.NewGuid();
            var projectId = Guid.NewGuid();
            var command = new ChangeProjectStatusCommand
            {
                ProjectId = projectId.ToString(),
                Status = ProjectStatus.Completed
            };

            var fakeProject = new ProjectBuilder()
                .WithId(projectId)
                .WithTitle("App Mobile")
                .WithDates(DateTime.Now, DateTime.Now.AddDays(5))
                .ManagedBy(Guid.NewGuid())
                .Build();

            _sessionServiceMock.Setup(s => s.GetCurrentUserID()).Returns(adminId);
            _projectServiceMock.Setup(s => s.ChangeProjectStatusAsync(command)).ReturnsAsync(fakeProject);

            var result = await _sut.HandleAsync(command);

            result.Should().NotBeNull();

            // Verificamos se a notificação descreve a mudança de estado
            _notificationSenderMock.Verify(n => n.DeliverAsync(It.Is<Notification>(notif =>
                notif.UserId == adminId &&
                notif.Message.Contains("App Mobile") &&
                notif.Message.Contains("Completed")
            )), Times.Once);
        }
    }
}