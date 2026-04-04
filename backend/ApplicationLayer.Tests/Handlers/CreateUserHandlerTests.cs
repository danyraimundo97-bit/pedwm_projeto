using System;
using System.Threading.Tasks;
using ApplicationLayer.Commands;
using ApplicationLayer.Handlers;
using ApplicationLayer.Services;
using DomainLayer.Domain.Users;
using DomainLayer.Domain.Notifications;
using DomainLayer.Domain.Builders;
using Moq;
using FluentAssertions;
using Xunit;

namespace ApplicationLayer.Tests.Handlers
{
    public class CreateUserHandlerTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<INotificationService> _notificationSenderMock;
        private readonly CreateUserHandler _sut;

        public CreateUserHandlerTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _notificationSenderMock = new Mock<INotificationService>();

            _sut = new CreateUserHandler(
                _userServiceMock.Object, 
                _notificationSenderMock.Object);
        }

        [Fact]
        public async Task HandleAsync_ShouldSendNotification_WhenUserCreated()
        {
            var command = new CreateUserCommand { Name = "Ana Silva" };
            var userId = Guid.NewGuid();

            var fakeUser = new UserBuilder()
                .WithId(userId)
                .WithName("Ana Silva")
                .Build();

            _userServiceMock.Setup(s => s.CreateUserAsync(command)).ReturnsAsync(fakeUser);

            var result = await _sut.HandleAsync(command);

            result.Should().NotBeNull();

            _notificationSenderMock.Verify(n => n.DeliverAsync(It.Is<Notification>(notif =>
                notif.UserId == userId && // Validar o ID
                notif.Message.Contains("Ana Silva")
            )), Times.Once);
        }
    }
}