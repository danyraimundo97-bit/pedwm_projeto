using System;
using System.Threading.Tasks;
using ApplicationLayer.Repositories;
using ApplicationLayer.Services;
using DomainLayer.Domain.Tasks;
using DomainLayer.Domain.Users;
using DomainLayer.Domain.Builders;
using Moq;
using FluentAssertions;
using Xunit;

namespace ApplicationLayer.Tests.Services
{
    public class TaskServiceTests
    {
        private readonly Mock<ITaskRepository> _taskRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IAppLogger> _loggerMock;
        private readonly TaskService _sut;

        public TaskServiceTests()
        {
            _taskRepositoryMock = new Mock<ITaskRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _loggerMock = new Mock<IAppLogger>();

            // O serviço precisa dos dois repositórios para garantir que a Tarefa e o User existem
            _sut = new TaskService(
                _taskRepositoryMock.Object,
                _userRepositoryMock.Object,
                _loggerMock.Object);
        }

        // ==========================================
        // TESTE 1: IDs Vazios
        // ==========================================
        [Fact]
        public async Task AssignUserToTask_ShouldThrowArgumentException_WhenAnyIdIsEmpty()
        {
            string userId = "";
            string taskId = "";
            string projectId = "";

            Func<Task> act = async () => await _sut.AssignUserToTaskAsync(userId, taskId, projectId);

            await act.Should().ThrowAsync<ArgumentException>();
        }

        // ==========================================
        // TESTE 2: Tarefa não encontrada
        // ==========================================
        [Fact]
        public async Task AssignUserToTask_ShouldThrowInvalidOperationException_WhenTaskNotFound()
        {
            var userId = Guid.NewGuid().ToString();
            var taskId = Guid.NewGuid().ToString();
            var projectId = Guid.NewGuid().ToString();

            // Simular que a tarefa não existe na BD (devolve nulo)
            _taskRepositoryMock.Setup(repo => repo.GetTaskAsync(taskId, projectId)).ReturnsAsync((TaskBase)null);

            Func<Task> act = async () => await _sut.AssignUserToTaskAsync(userId, taskId, projectId);

            await act.Should().ThrowAsync<InvalidOperationException>();
        }

        // ==========================================
        // TESTE 3: Sucesso
        // ==========================================
        [Fact]
        public async Task AssignUserToTask_ShouldAssignAndSave_WhenValid()
        {
            var userId = Guid.NewGuid();
            var taskId = Guid.NewGuid();
            var projectId = Guid.NewGuid();

            var fakeUser = new UserBuilder()
                .WithId(userId)
                .WithName("Maria")
                .Build();

            var fakeTask = new FeatureTaskBuilder()
                .WithId(taskId)
                .WithTitle("Criar Login")
                .InProject(projectId)
                .Build();

            _taskRepositoryMock.Setup(repo => repo.GetTaskAsync(taskId.ToString(), projectId.ToString())).ReturnsAsync(fakeTask);

            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(fakeUser);

            var result = await _sut.AssignUserToTaskAsync(userId.ToString(), taskId.ToString(), projectId.ToString());

            result.Should().NotBeNull();

            // Garantir que a tarefa mandou guardar as alterações!
            _taskRepositoryMock.Verify(repo => repo.SaveAsync(It.IsAny<TaskBase>()), Times.Once);
        }
    }
}