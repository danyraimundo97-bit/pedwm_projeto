using ApplicationLayer.Commands;
using ApplicationLayer.Handlers;
using ApplicationLayer.Mapping;
using ApplicationLayer.Models;
using ApplicationLayer.Services;
using DomainLayer.Domain.Builders;
using FluentAssertions;
using HotChocolate;
using Moq;
using PresentationLayer.DTOs;
using PresentationLayer.GraphQL;
using System;
using System.Threading.Tasks;
using Xunit;

namespace PresentationLayer.Tests.GraphQL
{
    public class MutationTests
    {
        private readonly Mock<IAppLogger> _loggerMock;
        private readonly Mutation _sut;

        // Mocks para construir os Handlers
        private readonly Mock<IProjectService> _projectServiceMock;
        private readonly Mock<ITaskService> _taskServiceMock;
        private readonly Mock<ITeamService> _teamServiceMock;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<Mapper> _mapperMock;
        private readonly Mock<INotificationService> _notificationMock;
        private readonly Mock<ISessionService> _sessionMock;

        public MutationTests()
        {
            _loggerMock = new Mock<IAppLogger>();
            _sut = new Mutation(_loggerMock.Object);

            _projectServiceMock = new Mock<IProjectService>();
            _taskServiceMock = new Mock<ITaskService>();
            _teamServiceMock = new Mock<ITeamService>();
            _userServiceMock = new Mock<IUserService>();
            _mapperMock = new Mock<Mapper>();
            _notificationMock = new Mock<INotificationService>();
            _sessionMock = new Mock<ISessionService>();
        }

        // =================================================================
        // TESTE 1: CREATE PROJECT (3 CENÁRIOS POSSÍVEIS)
        // =================================================================

        [Fact]
        public async Task CreateProject_ShouldReturnSuccessString_WhenValid()
        {
            var input = new CreateProject_DTO { Title = "Novo Website" };
            var fakeProjectId = Guid.NewGuid();
            var fakeSender = new ProjectSender { Id = fakeProjectId, Title = "Novo Website" };

            var handler = new CreateProjectHandler(_projectServiceMock.Object, _mapperMock.Object, _notificationMock.Object, _sessionMock.Object);

            // Usar o Builder para criar o projeto
            var fakeProject = new ProjectBuilder()
                .WithId(fakeProjectId)
                .WithTitle("Novo Website")
                .WithDates(DateTime.Now, DateTime.Now.AddDays(30))
                .ManagedBy(Guid.NewGuid())
                .Build();

            // Simular que o Handler vai ter sucesso e devolver o ProjectSender
            _projectServiceMock.Setup(s => s.CreateProjectAsync(It.IsAny<CreateProjectCommand>()))
                .ReturnsAsync(fakeProject);

            _mapperMock.Setup(m => m.ToProjectSender(It.IsAny<DomainLayer.Domain.Projects.ProjectBase>()))
                .Returns(fakeSender);

            var result = await _sut.CreateProject(input, handler);

            result.Should().Be($"Projeto 'Novo Website' criado com o ID: {fakeProjectId}");
        }

        [Fact]
        public async Task CreateProject_ShouldThrowGraphQLException_WhenArgumentExceptionOccurs()
        {
            var input = new CreateProject_DTO { Title = "" };
            var handler = new CreateProjectHandler(_projectServiceMock.Object, _mapperMock.Object, _notificationMock.Object, _sessionMock.Object);

            // Simular um erro de validação (ex: Título vazio)
            _projectServiceMock.Setup(s => s.CreateProjectAsync(It.IsAny<CreateProjectCommand>()))
                               .ThrowsAsync(new ArgumentException("O título é obrigatório."));

            Func<Task> act = async () => await _sut.CreateProject(input, handler);

            await act.Should().ThrowAsync<GraphQLException>().WithMessage("O título é obrigatório.");
        }

        [Fact]
        public async Task CreateProject_ShouldLogAndThrowGenericException_WhenUnexpectedErrorOccurs()
        {
            var input = new CreateProject_DTO { Title = "Erro Fatal" };
            var handler = new CreateProjectHandler(
                _projectServiceMock.Object, _mapperMock.Object, _notificationMock.Object, _sessionMock.Object);

            // Simular que a base de dados foi abaixo (Exceção genérica)
            var dbError = new Exception("Falha de conexão com a BD.");
            _projectServiceMock.Setup(s => s.CreateProjectAsync(It.IsAny<CreateProjectCommand>()))
                               .ThrowsAsync(dbError);

            Func<Task> act = async () => await _sut.CreateProject(input, handler);

            await act.Should().ThrowAsync<GraphQLException>()
                .WithMessage("Ocorreu um erro interno inesperado ao tentar criar o projeto.");

            // Verificar se o logger gravou a mensagem corretamente!
            _loggerMock.Verify(l => l.LogError(It.Is<string>(msg => msg.Contains("Falha ao tentar criar o Projeto com o título 'Erro Fatal'")), dbError), Times.Once);
        }

        // =================================================================
        // TESTE 2: CREATE TASK
        // =================================================================
        [Fact]
        public async Task CreateTask_ShouldReturnSuccessString_WhenValid()
        {
            var input = new CreateTask_DTO { Title = "Nova Tarefa" };
            var fakeTaskId = Guid.NewGuid();
            var fakeSender = new TaskSender { Id = fakeTaskId, Title = "Nova Tarefa" };

            var handler = new CreateTaskHandler(_taskServiceMock.Object, _mapperMock.Object, _notificationMock.Object);

            var fakeTask = new FeatureTaskBuilder().WithId(fakeTaskId).WithTitle("Nova Tarefa").Build();

            _taskServiceMock.Setup(s => s.CreateTaskAsync(It.IsAny<CreateTaskCommand>())).ReturnsAsync(fakeTask);
            _mapperMock.Setup(m => m.ToTaskSender(It.IsAny<DomainLayer.Domain.Tasks.TaskBase>())).Returns(fakeSender);

            var result = await _sut.CreateTask(input, handler);

            result.Should().Be($"Tarefa 'Nova Tarefa' criada com o ID: {fakeTaskId}");
        }

        // =================================================================
        // TESTE 3: CREATE USER
        // =================================================================
        [Fact]
        public async Task CreateUser_ShouldReturnSuccessString_WhenValid()
        {
            var input = new CreateUser_DTO { Name = "João", Email = "joao@teste.com" };
            var handler = new CreateUserHandler(_userServiceMock.Object, _notificationMock.Object);

            var fakeUser = new UserBuilder().WithId(Guid.NewGuid()).WithName("João").WithEmail("joao@teste.com").Build();

            _userServiceMock.Setup(s => s.CreateUserAsync(It.IsAny<CreateUserCommand>())).ReturnsAsync(fakeUser);

            var result = await _sut.CreateUser(input, handler);

            result.Should().Be("Utilizador 'João' criado com o email: joao@teste.com");
        }

        // =================================================================
        // TESTE 4: CREATE TEAM
        // =================================================================
        [Fact]
        public async Task CreateTeam_ShouldReturnSuccessString_WhenValid()
        {
            var input = new CreateTeam_DTO { Name = "Backend" };
            var handler = new CreateTeamHandler(_teamServiceMock.Object, _notificationMock.Object);

            var fakeTeam = new TeamBuilder().WithId(Guid.NewGuid()).WithName("Backend").Build();

            _teamServiceMock.Setup(s => s.CreateTeamAsync(It.IsAny<CreateTeamCommand>())).ReturnsAsync(fakeTeam);

            var result = await _sut.CreateTeam(input, handler);

            result.Should().Be("Equipa 'Backend' criada com sucesso");
        }

        // =================================================================
        // TESTE 5: CHANGE PROJECT STATUS
        // =================================================================
        [Fact]
        public async Task ChangeProjectStatus_ShouldReturnSuccessString_WhenValid()
        {
            var input = new ChangeProjectStatus_DTO { ProjectId = Guid.NewGuid().ToString(), Status = DomainLayer.Domain.Projects.ProjectStatus.Completed };
            var handler = new ChangeProjectStatusHandler(_sessionMock.Object, _projectServiceMock.Object, _notificationMock.Object);

            var fakeProject = new ProjectBuilder().WithId(Guid.NewGuid()).WithTitle("App iOS").WithDates(DateTime.Now, DateTime.Now).ManagedBy(Guid.NewGuid()).Build();

            _projectServiceMock.Setup(s => s.ChangeProjectStatusAsync(It.IsAny<ChangeProjectStatusCommand>())).ReturnsAsync(fakeProject);

            var result = await _sut.ChangeProjectStatus(input, handler);

            result.Should().Be("Estado do projeto 'App iOS' atualizado para Completed.");
        }

        // =================================================================
        // TESTE 6: ASSIGN TASK TO USER (InvalidOperationException)
        // =================================================================

        [Fact]
        public async Task AssignTaskToUser_ShouldThrowGraphQLException_WhenInvalidOperationExceptionOccurs()
        {
            var input = new AssignUserToTask_DTO { TaskId = Guid.NewGuid().ToString(), AssigneeUserId = Guid.NewGuid().ToString() };

            var handler = new AssignUserToTaskHandler(_sessionMock.Object, _taskServiceMock.Object, _notificationMock.Object);

            // Simular erro  na regra de negócio (ex: Tarefa já fechada ou não encontrada)
            _taskServiceMock.Setup(s => s.AssignUserToTaskAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                            .ThrowsAsync(new InvalidOperationException("A tarefa não foi encontrada."));

            Func<Task> act = async () => await _sut.AssignTaskToUser(input, handler);

            await act.Should().ThrowAsync<GraphQLException>().WithMessage("A tarefa não foi encontrada.");
        }

        // =================================================================
        // ASSIGN USER TO TEAM
        // =================================================================
        [Fact]
        public async Task AssignUserToTeam_ShouldReturnSuccessString_WhenValid()
        {
            var input = new AssignUserToTeam_DTO { UserId = Guid.NewGuid().ToString(), TeamId = Guid.NewGuid().ToString() };
            var handler = new AssignUserToTeamHandler(_sessionMock.Object, _userServiceMock.Object, _notificationMock.Object);

            var fakeUser = new UserBuilder().WithId(Guid.NewGuid()).WithName("Maria").Build();

            _userServiceMock.Setup(s => s.AssignUserToTeamAsync(It.IsAny<AssignUserToTeamCommand>())).ReturnsAsync(fakeUser);

            var result = await _sut.AssignUserToTeam(input, handler);

            result.Should().Be("Utilizador 'Maria' associado à equipa com sucesso.");
        }

        // =================================================================
        // ADD HOURS TO PROJECT
        // =================================================================
        [Fact]
        public async Task AddHoursToProject_ShouldReturnSuccessString_WhenValid()
        {
            var input = new AddHoursToProject_DTO { ProjectId = Guid.NewGuid().ToString(), Hours = 5 };
            var handler = new AddHoursToProjectHandler(_sessionMock.Object, _projectServiceMock.Object, _notificationMock.Object);

            var fakeProject = new ProjectBuilder().WithId(Guid.NewGuid()).WithTitle("App iOS").WithDates(DateTime.Now, DateTime.Now).ManagedBy(Guid.NewGuid()).Build();

            _projectServiceMock.Setup(s => s.AddConsumedHoursToProjectAsync(It.IsAny<AddHoursToProjectCommand>())).ReturnsAsync(fakeProject);

            var result = await _sut.AddHoursToProject(input, handler);

            result.Should().Be("Registadas 5 horas no projeto 'App iOS'.");
        }
    }
}