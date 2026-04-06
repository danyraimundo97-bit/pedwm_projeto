using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using HotChocolate;
using FluentAssertions;
using Moq;

using ApplicationLayer.Repositories;
using ApplicationLayer.Queries;
using ApplicationLayer.Mapping;
using ApplicationLayer.Models;
using DomainLayer.Domain.Builders;
using DomainLayer.Domain.Projects;
using DomainLayer.Domain.Tasks;
using DomainLayer.Domain.Teams;
using DomainLayer.Domain.Users;
using PresentationLayer.GraphQL;

namespace PresentationLayer.Tests.GraphQL
{
    public class QueryTests
    {
        private readonly Query _sut;

        // Mock dos repositórios
        private readonly Mock<IProjectRepository> _projectRepoMock;
        private readonly Mock<ITaskRepository> _taskRepoMock;
        private readonly Mock<ITeamRepository> _teamRepoMock;
        private readonly Mock<IUserRepository> _userRepoMock;
        // Mock do Mapper (Mapster)
        private readonly Mock<Mapper> _mapperMock;

        public QueryTests()
        {
            _sut = new Query();
            _projectRepoMock = new Mock<IProjectRepository>();
            _taskRepoMock = new Mock<ITaskRepository>();
            _teamRepoMock = new Mock<ITeamRepository>();
            _userRepoMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<Mapper>();
        }

        // ==========================================
        // PROJECTS
        // ==========================================

        // ==========================================
        // TESTE 1: Exceção GraphQLException

        [Fact]
        public async Task GetProjectById_ShouldThrowGraphQLException_WhenNotFound()
        {
            var invalidId = Guid.NewGuid();

            // Simular que o repositório devolve nulo
            _projectRepoMock.Setup(repo => repo.GetByIdAsync(invalidId)).ReturnsAsync((ProjectBase)null);

            var handler = new GetProjectByIdQueryHandler(_projectRepoMock.Object, _mapperMock.Object);
            
            Func<Task> act = async () => await _sut.GetProjectById(invalidId, handler);

            await act.Should().ThrowAsync<GraphQLException>()
                .WithMessage($"O Projeto com o ID {invalidId} não foi encontrado.");
        }

        // ==========================================
        // TESTE 2: Sucesso e Mapeamento Mapster

        [Fact]
        public async Task GetProjectById_ShouldReturnProjectSender_WhenExists()
        {
            var projectId = Guid.NewGuid();

            var fakeProject = new ProjectBuilder()
                .WithId(projectId)
                .WithTitle("Website XPTO")
                .WithDates(DateTime.Now, DateTime.Now.AddDays(5))
                .ManagedBy(Guid.NewGuid())
                .Build();

            var fakeSender = new ProjectSender { Id = projectId, Title = "Website XPTO" };

            _projectRepoMock.Setup(repo => repo.GetByIdAsync(projectId)).ReturnsAsync(fakeProject);
            _mapperMock.Setup(m => m.ToProjectSender(fakeProject)).Returns(fakeSender);

            var handler = new GetProjectByIdQueryHandler(_projectRepoMock.Object, _mapperMock.Object);

            // Injetar o mock do repositório diretamente no método
            var result = await _sut.GetProjectById(projectId, handler);

            result.Should().NotBeNull();
            result.Should().BeOfType<ProjectSender>();
            result.Title.Should().Be("Website XPTO");
        }

        // ==========================================
        // TESTE 3: Paginação (Lista)

        [Fact]
        public async Task GetProjects_ShouldReturnMappedList_WhenCalled()
        {
            var fakeList = new List<ProjectBase>{new ProjectBuilder().WithId(Guid.NewGuid()).WithTitle("Proj 1").WithDates(DateTime.Now, DateTime.Now).ManagedBy(Guid.NewGuid()).Build()};
            var fakeSender = new ProjectSender { Title = "Proj 1" };

            _projectRepoMock.Setup(repo => repo.GetPagedAsync(1, 100)).ReturnsAsync(fakeList);
            _mapperMock.Setup(m => m.ToProjectSender(It.IsAny<ProjectBase>())).Returns(fakeSender);

            var handler = new ListProjectsQueryHandler(_projectRepoMock.Object, _mapperMock.Object);

            var result = await _sut.GetProjects(handler);

            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result[0].Title.Should().Be("Proj 1");
        }

        // ==========================================
        // TESTE 4: (Filtros)

        [Fact]
        public async Task GetProjectsByUser_ShouldReturnMappedList_WhenCalled()
        {
            var userId = Guid.NewGuid();
            var fakeList = new List<ProjectBase>{new ProjectBuilder().WithId(Guid.NewGuid()).WithTitle("Proj User").WithDates(DateTime.Now, DateTime.Now).ManagedBy(userId).Build()};
            var fakeSender = new ProjectSender { Title = "Proj User" };

            _projectRepoMock.Setup(repo => repo.GetByUserAsync(userId)).ReturnsAsync(fakeList);
            _mapperMock.Setup(m => m.ToProjectSender(It.IsAny<ProjectBase>())).Returns(fakeSender);

            var handler = new GetProjectsByUserQueryHandler(_projectRepoMock.Object, _mapperMock.Object);

            var result = await _sut.GetProjectsByUser(userId, handler);

            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result[0].Title.Should().Be("Proj User");
        }

        // ==========================================
        // TAREFAS
        // ==========================================

        // ==========================================
        // TESTE 1: Exceção GraphQLException

        [Fact]
        public async Task GetTaskById_ShouldThrowGraphQLException_WhenNotFound()
        {
            var invalidId = Guid.NewGuid();
            _taskRepoMock.Setup(repo => repo.GetByIdAsync(invalidId)).ReturnsAsync((TaskBase)null);

            var handler = new GetTaskByIdQueryHandler(_taskRepoMock.Object, _mapperMock.Object);

            Func<Task> act = async () => await _sut.GetTaskById(invalidId, handler);

            await act.Should().ThrowAsync<GraphQLException>()
                .WithMessage($"A Tarefa com o ID {invalidId} não foi encontrada.");
        }

        // ==========================================
        // TESTE 2: Sucesso e Mapeamento Mapster

        [Fact]
        public async Task GetTaskById_ShouldReturnTaskSender_WhenExists()
        {
            var taskId = Guid.NewGuid();
            var fakeTask = new FeatureTaskBuilder().WithId(taskId).WithTitle("Criar API").Build();
            var fakeSender = new TaskSender { Id = taskId, Title = "Criar API" };

            _taskRepoMock.Setup(repo => repo.GetByIdAsync(taskId)).ReturnsAsync(fakeTask);
            _mapperMock.Setup(m => m.ToTaskSender(fakeTask)).Returns(fakeSender);

            var handler = new GetTaskByIdQueryHandler(_taskRepoMock.Object, _mapperMock.Object);

            var result = await _sut.GetTaskById(taskId, handler);

            result.Should().NotBeNull();
            result.Should().BeOfType<TaskSender>();
            result.Title.Should().Be("Criar API");
        }

        // ==========================================
        // TESTE 3: Paginação (Lista)

        [Fact]
        public async Task GetTasks_ShouldReturnMappedList_WhenCalled()
        {
            var fakeList = new List<TaskBase> { new FeatureTaskBuilder().WithTitle("Task A").Build() };

            _taskRepoMock.Setup(repo => repo.GetPagedAsync(1, 100)).ReturnsAsync(fakeList);
            _mapperMock.Setup(m => m.ToTaskSender(It.IsAny<TaskBase>())).Returns(new TaskSender());

            var handler = new ListTasksQueryHandler(_taskRepoMock.Object, _mapperMock.Object);

            var result = await _sut.GetTasks(handler);

            result.Should().NotBeNull();
            result.Should().HaveCount(1);
        }

        // ==========================================
        // TESTE 4: (Filtros)

        [Fact]
        public async Task GetTasksByProject_ShouldReturnMappedList_WhenCalled()
        {
            var projectId = Guid.NewGuid();
            var fakeList = new List<TaskBase> { new FeatureTaskBuilder().WithTitle("Task B").Build() };

            _taskRepoMock.Setup(repo => repo.GetByProjectAsync(projectId)).ReturnsAsync(fakeList);
            _mapperMock.Setup(m => m.ToTaskSender(It.IsAny<TaskBase>())).Returns(new TaskSender());

            var handler = new GetTasksByProjectQueryHandler(_taskRepoMock.Object, _mapperMock.Object);
            
            var result = await _sut.GetTasksByProject(projectId, handler);

            result.Should().NotBeNull();
            result.Should().HaveCount(1);
        }

        [Fact]
        public async Task GetTasksByUser_ShouldReturnMappedList_WhenCalled()
        {
            var userId = Guid.NewGuid();
            var fakeList = new List<TaskBase> { new FeatureTaskBuilder().WithTitle("Task Do User").Build() };

            _taskRepoMock.Setup(repo => repo.GetByUserAsync(userId)).ReturnsAsync(fakeList);
            _mapperMock.Setup(m => m.ToTaskSender(It.IsAny<TaskBase>())).Returns(new TaskSender());

            var handler = new GetTasksByUserQueryHandler(_taskRepoMock.Object, _mapperMock.Object);
            
            var result = await _sut.GetTasksByUser(userId, handler);

            result.Should().NotBeNull();
            result.Should().HaveCount(1);
        }


        // ==========================================
        // EQUIPAS
        // ==========================================

        // ==========================================
        // TESTE 1: Exceção GraphQLException

        [Fact]
        public async Task GetTeamById_ShouldThrowGraphQLException_WhenNotFound()
        {
            var invalidId = Guid.NewGuid();
            _teamRepoMock.Setup(repo => repo.GetByIdAsync(invalidId)).ReturnsAsync((Team)null);

            var handler = new GetTeamByIdQueryHandler(_teamRepoMock.Object, _mapperMock.Object);

            Func<Task> act = async () => await _sut.GetTeamById(invalidId, handler);

            await act.Should().ThrowAsync<GraphQLException>()
                .WithMessage($"A Equipa com o ID {invalidId} não foi encontrada.");
        }

        // ==========================================
        // TESTE 2: Sucesso e Mapeamento Mapster

        [Fact]
        public async Task GetTeamById_ShouldReturnTeamSender_WhenExists()
        {
            var teamId = Guid.NewGuid();
            var fakeTeam = new TeamBuilder().WithId(teamId).WithName("Data Science").Build();
            var fakeSender = new TeamSender { Id = teamId, Name = "Data Science" };

            _teamRepoMock.Setup(repo => repo.GetByIdAsync(teamId)).ReturnsAsync(fakeTeam);
            _mapperMock.Setup(m => m.ToTeamSender(fakeTeam)).Returns(fakeSender);

            var handler = new GetTeamByIdQueryHandler(_teamRepoMock.Object, _mapperMock.Object);
            
            var result = await _sut.GetTeamById(teamId, handler);

            result.Should().NotBeNull();
            result.Should().BeOfType<TeamSender>();
            result.Name.Should().Be("Data Science");
        }

        // ==========================================
        // TESTE 3: Paginação (Lista)

        [Fact]
        public async Task GetTeams_ShouldReturnMappedList_WhenCalled()
        {
            var fakeList = new List<Team> { new TeamBuilder().WithName("Equipa Alpha").Build() };

            _teamRepoMock.Setup(repo => repo.GetPagedAsync(1, 100)).ReturnsAsync(fakeList);
            _mapperMock.Setup(m => m.ToTeamSender(It.IsAny<Team>())).Returns(new TeamSender());

            var handler = new ListTeamsQueryHandler(_teamRepoMock.Object, _mapperMock.Object);
            
            var result = await _sut.GetTeams(handler);

            result.Should().NotBeNull();
            result.Should().HaveCount(1);
        }


        // ==========================================
        // UTILIZADORES
        // ==========================================

        // ==========================================
        // TESTE 1: Exceção GraphQLException

        [Fact]
        public async Task GetUserById_ShouldThrowGraphQLException_WhenNotFound()
        {
            var invalidId = Guid.NewGuid();
            _userRepoMock.Setup(repo => repo.GetByIdAsync(invalidId)).ReturnsAsync((User)null);

            var handler = new GetUserByIdQueryHandler(_userRepoMock.Object, _mapperMock.Object);

            Func<Task> act = async () => await _sut.GetUserById(invalidId, handler);

            await act.Should().ThrowAsync<GraphQLException>()
                .WithMessage($"O Utilizador com o ID {invalidId} não foi encontrado.");
        }

        // ==========================================
        // TESTE 2: Sucesso e Mapeamento Mapster

        [Fact]
        public async Task GetUserById_ShouldReturnUserResponse_WhenExists()
        {
            var userId = Guid.NewGuid();
            var fakeUser = new UserBuilder().WithId(userId).WithName("Tiago").WithEmail("tiago@teste.com").Build();
            var fakeResponse = new UserResponse { Id = userId, Name = "Tiago", Email = "tiago@teste.com" };

            _userRepoMock.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(fakeUser);
            _mapperMock.Setup(m => m.ToUserSender(fakeUser)).Returns(fakeResponse);

            var handler = new GetUserByIdQueryHandler(_userRepoMock.Object, _mapperMock.Object);
            
            var result = await _sut.GetUserById(userId, handler);

            result.Should().NotBeNull();
            result.Should().BeOfType<UserResponse>();
            result.Name.Should().Be("Tiago");
        }

        // ==========================================
        // TESTE 3: Paginação (Lista)

        [Fact]
        public async Task GetUsers_ShouldReturnMappedList_WhenCalled()
        {
            var fakeList = new List<User> { new UserBuilder().WithName("Rita").Build() };

            _userRepoMock.Setup(repo => repo.GetPagedAsync(1, 100)).ReturnsAsync(fakeList);
            _mapperMock.Setup(m => m.ToUserSender(It.IsAny<User>())).Returns(new UserResponse());

            var handler = new ListUsersQueryHandler(_userRepoMock.Object, _mapperMock.Object);
            var result = await _sut.GetUsers(handler);

            result.Should().NotBeNull();
            result.Should().HaveCount(1);
        }
    }
}