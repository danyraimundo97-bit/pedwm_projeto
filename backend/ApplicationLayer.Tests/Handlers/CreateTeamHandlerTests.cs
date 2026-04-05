using System;
using System.Threading.Tasks;
using ApplicationLayer.Commands;
using ApplicationLayer.Handlers;
using ApplicationLayer.Services;
using DomainLayer.Domain.Teams;
using DomainLayer.Domain.Notifications;
using DomainLayer.Domain.Builders;
using Moq;
using FluentAssertions;
using Xunit;

namespace ApplicationLayer.Tests.Handlers
{
    public class CreateTeamHandlerTests
    {
        private readonly Mock<ITeamService> _teamServiceMock;
        private readonly Mock<ApplicationLayer.Services.INotificationService> _notificationSenderMock;
        private readonly CreateTeamHandler _sut;

        public CreateTeamHandlerTests()
        {
            _teamServiceMock = new Mock<ITeamService>();
            _notificationSenderMock = new Mock<ApplicationLayer.Services.INotificationService>();

            _sut = new CreateTeamHandler(
                _teamServiceMock.Object, 
                _notificationSenderMock.Object);
        }

        [Fact]
        public async Task HandleAsync_ShouldSendNotification_WhenTeamCreated()
        {
            var command = new CreateTeamCommand { Name = "DevOps" };
            var fakeTeam = new TeamBuilder().WithId(Guid.NewGuid()).WithName("DevOps").Build();

            _teamServiceMock.Setup(s => s.CreateTeamAsync(command)).ReturnsAsync(fakeTeam);

            var result = await _sut.HandleAsync(command);

            result.Should().NotBeNull();
            _notificationSenderMock.Verify(n => n.DeliverAsync(It.Is<Notification>(notif =>
                notif.Message.Contains("DevOps")
            )), Times.Once);
        }
    }
}