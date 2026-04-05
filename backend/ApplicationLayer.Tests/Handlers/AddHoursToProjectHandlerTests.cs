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
    public class AddHoursToProjectHandlerTests
    {
        private readonly Mock<ISessionService> _sessionServiceMock;
        private readonly Mock<IProjectService> _projectServiceMock;
        private readonly Mock<INotificationService> _notificationSenderMock;
        private readonly AddHoursToProjectHandler _sut;

        public AddHoursToProjectHandlerTests()
        {
            _sessionServiceMock = new Mock<ISessionService>();
            _projectServiceMock = new Mock<IProjectService>();
            _notificationSenderMock = new Mock<INotificationService>();

            _sut = new AddHoursToProjectHandler(
                _sessionServiceMock.Object,
                _projectServiceMock.Object,
                _notificationSenderMock.Object);
        }

        // ==========================================
        // TESTE 1: Serviço Falha -> nenhuma notificação deve ser enviada
        // ==========================================
        [Fact]
        public async Task HandleAsync_ShouldNotSendNotification_WhenServiceThrowsException()
        {
            var command = new AddHoursToProjectCommand
            {
                ProjectId = Guid.NewGuid().ToString(),
                Hours = -5 // Horas negativas
            };

            // Simulamos que o ProjectService ativa uma exceção de validação
            _projectServiceMock
                .Setup(s => s.AddConsumedHoursToProjectAsync(command))
                .ThrowsAsync(new ArgumentException("As horas devem ser um valor positivo."));

            Func<Task> act = async () => await _sut.HandleAsync(command);

            await act.Should().ThrowAsync<ArgumentException>();

            // A notificação não é enviada
            _notificationSenderMock.Verify(n => n.DeliverAsync(It.IsAny<Notification>()), Times.Never);
        }

        // ==========================================
        // TESTE 2: Sucesso -> A notificação deve ser enviada
        // ==========================================
        [Fact]
        public async Task HandleAsync_ShouldSendNotification_WhenSuccessful()
        {
            var adminId = Guid.NewGuid();
            var projectId = Guid.NewGuid();

            var command = new AddHoursToProjectCommand
            {
                ProjectId = projectId.ToString(),
                Hours = 10
            };

            var fakeProject = new ProjectBuilder()
                .WithId(projectId)
                .WithTitle("Novo Website")
                .WithDates(DateTime.Now, DateTime.Now.AddDays(30))
                .ManagedBy(Guid.NewGuid())
                .Build();

            // Simulamos a sessão adminId
            _sessionServiceMock.Setup(s => s.GetCurrentUserID()).Returns(adminId);

            // Simulamos o serviço a devolver o projeto atualizado
            _projectServiceMock.Setup(s => s.AddConsumedHoursToProjectAsync(command)).ReturnsAsync(fakeProject);

            var result = await _sut.HandleAsync(command);

            result.Should().NotBeNull();
            result.Title.Should().Be("Novo Website");

            _notificationSenderMock.Verify(n => n.DeliverAsync(It.Is<Notification>(notif =>
                notif.UserId == adminId &&
                notif.Type == NotificationType.Info &&
                notif.Message.Contains("10 horas") &&
                notif.Message.Contains("Novo Website")
            )), Times.Once);
        }
    }
}