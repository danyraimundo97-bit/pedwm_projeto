using System;
using System.Threading.Tasks;
using ApplicationLayer.Commands;
using ApplicationLayer.Handlers;
using ApplicationLayer.Mapping;
using ApplicationLayer.Models;
using ApplicationLayer.Services;
using DomainLayer.Domain.Projects;
using DomainLayer.Domain.Notifications;
using DomainLayer.Domain.Builders;
using Moq;
using FluentAssertions;
using Xunit;

namespace ApplicationLayer.Tests.Handlers
{
    public class CreateProjectHandlerTests
    {
        private readonly Mock<IProjectService> _projectServiceMock;
        private readonly Mock<Mapper> _mapperMock;
        private readonly Mock<INotificationService> _notificationSenderMock;
        private readonly Mock<ISessionService> _sessionServiceMock;
        private readonly CreateProjectHandler _sut;

        public CreateProjectHandlerTests()
        {
            _projectServiceMock = new Mock<IProjectService>();
            _mapperMock = new Mock<Mapper>();
            _notificationSenderMock = new Mock<INotificationService>();
            _sessionServiceMock = new Mock<ISessionService>();

            _sut = new CreateProjectHandler(
                _projectServiceMock.Object,
                _mapperMock.Object,
                _notificationSenderMock.Object,
                _sessionServiceMock.Object);
        }

        [Fact]
        public async Task HandleAsync_ShouldSendNotification_WhenProjectCreated()
        {
            var adminId = Guid.NewGuid();
            var command = new CreateProjectCommand();

            var fakeProject = new ProjectBuilder()
                .WithId(Guid.NewGuid())
                .WithTitle("Novo ERP")
                .WithDates(DateTime.Now, DateTime.Now.AddDays(10))
                .ManagedBy(adminId)
                .Build();

            var fakeSender = new ProjectResponse { Title = "Novo ERP" };

            _sessionServiceMock.Setup(s => s.GetCurrentUserID()).Returns(adminId);
            _projectServiceMock.Setup(s => s.CreateProjectAsync(command)).ReturnsAsync(fakeProject);

            // Mapper devolve o fakeSender!
            _mapperMock.Setup(m => m.ToProjectResponse(fakeProject)).Returns(fakeSender);

            var result = await _sut.HandleAsync(command);

            result.Should().NotBeNull();
            result.Title.Should().Be("Novo ERP");

            _notificationSenderMock.Verify(n => n.DeliverAsync(It.Is<Notification>(notif =>
                notif.UserId == adminId &&
                notif.Message.Contains("Novo ERP")
            )), Times.Once);
        }
    }
}