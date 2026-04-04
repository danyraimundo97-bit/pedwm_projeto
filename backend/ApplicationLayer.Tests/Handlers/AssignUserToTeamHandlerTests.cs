using System;
using System.Threading.Tasks;
using ApplicationLayer.Commands;
using ApplicationLayer.Handlers;
using ApplicationLayer.Services;
using DomainLayer.Domain.Notifications;
using DomainLayer.Domain.Users;
using DomainLayer.Domain.Builders;
using Moq;
using FluentAssertions;
using Xunit;

namespace ApplicationLayer.Tests.Handlers
{
    public class AssignUserToTeamHandlerTests
    {
        private readonly Mock<ISessionService> _sessionServiceMock;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<INotificationService> _notificationSenderMock;
        private readonly AssignUserToTeamHandler _sut;

        public AssignUserToTeamHandlerTests()
        {
            _sessionServiceMock = new Mock<ISessionService>();
            _userServiceMock = new Mock<IUserService>();
            _notificationSenderMock = new Mock<INotificationService>();

            _sut = new AssignUserToTeamHandler(
                _sessionServiceMock.Object,
                _userServiceMock.Object,
                _notificationSenderMock.Object);
        }

        // ==========================================
        // TESTE 1: Serviço Falha -> nenhuma notificação deve ser enviada
        // ==========================================
        [Fact]
        public async Task HandleAsync_ShouldNotSendNotification_WhenUserServiceThrowsException()
        {
            var command = new AssignUserToTeamCommand
            {
                UserId = Guid.NewGuid().ToString(),
                TeamId = Guid.NewGuid().ToString()
            };

            // Simulamos que o UserService encontrou um erro
            _userServiceMock
                .Setup(s => s.AssignUserToTeamAsync(command))
                .ThrowsAsync(new InvalidOperationException("Utilizador não encontrado."));

            Func<Task> act = async () => await _sut.HandleAsync(command);

            await act.Should().ThrowAsync<InvalidOperationException>();

            // A notificação não pode ser enviada em caso de erro
            _notificationSenderMock.Verify(n => n.DeliverAsync(It.IsAny<Notification>()), Times.Never);
        }

        // ==========================================
        // TESTE 2: Sucesso -> A notificação deve ser enviada
        // ==========================================
        [Fact]
        public async Task HandleAsync_ShouldSendNotification_WhenSuccessful()
        {
            var adminId = Guid.NewGuid();
            var command = new AssignUserToTeamCommand
            {
                UserId = Guid.NewGuid().ToString(),
                TeamId = Guid.NewGuid().ToString()
            };

            var fakeUser = new UserBuilder()
                .WithId(Guid.Parse(command.UserId))
                .WithName("Rui")
                .Build();

            // Simulamos a sessão (adminId)
            _sessionServiceMock.Setup(s => s.GetCurrentUserID()).Returns(adminId);

            // Simulamos o serviço a devolver o user
            _userServiceMock.Setup(s => s.AssignUserToTeamAsync(command)).ReturnsAsync(fakeUser);

            var result = await _sut.HandleAsync(command);

            // O resultado não pode ser nulo
            result.Should().NotBeNull();
            result.Name.Should().Be("Rui");

            _notificationSenderMock.Verify(n => n.DeliverAsync(It.Is<Notification>(notif =>
                notif.UserId == adminId &&
                notif.Type == NotificationType.Info &&
                notif.Message.Contains("adicionado à equipa")
            )), Times.Once);
        }
    }
}