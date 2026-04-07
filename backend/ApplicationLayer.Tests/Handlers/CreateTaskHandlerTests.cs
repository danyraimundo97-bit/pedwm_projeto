using System;
using System.Threading.Tasks;
using ApplicationLayer.Commands;
using ApplicationLayer.Handlers;
using ApplicationLayer.Mapping;
using ApplicationLayer.Models;
using ApplicationLayer.Services;
using DomainLayer.Domain.Tasks;
using DomainLayer.Domain.Notifications;
using DomainLayer.Domain.Builders;
using Moq;
using FluentAssertions;
using Xunit;

namespace ApplicationLayer.Tests.Handlers
{
    public class CreateTaskHandlerTests
    {
        private readonly Mock<ITaskService> _taskServiceMock;
        private readonly Mock<Mapper> _mapperMock;
        private readonly Mock<INotificationService> _notificationSenderMock;
        private readonly CreateTaskHandler _sut;

        public CreateTaskHandlerTests()
        {
            _taskServiceMock = new Mock<ITaskService>();
            _mapperMock = new Mock<Mapper>();
            _notificationSenderMock = new Mock<INotificationService>();

            _sut = new CreateTaskHandler(
                _taskServiceMock.Object,
                _mapperMock.Object,
                _notificationSenderMock.Object);
        }

        [Fact]
        public async Task HandleAsync_ShouldSendNotification_WhenTaskCreated()
        {
            var command = new CreateTaskCommand();
            var fakeTask = new FeatureTaskBuilder()
                .InProject(Guid.NewGuid())
                .WithId(Guid.NewGuid())
                .WithTitle("Configurar BD")
                .Build();
            var fakeSender = new TaskResponse { Title = "Configurar BD" };

            _taskServiceMock.Setup(s => s.CreateTaskAsync(command)).ReturnsAsync(fakeTask);
            _mapperMock.Setup(m => m.ToTaskResponse(fakeTask)).Returns(fakeSender);

            var result = await _sut.HandleAsync(command);

            result.Should().NotBeNull();
            result.Title.Should().Be("Configurar BD");

            _notificationSenderMock.Verify(n => n.DeliverAsync(It.Is<Notification>(notif =>
                notif.Message.Contains("Configurar BD")
            )), Times.Once);
        }
    }
}