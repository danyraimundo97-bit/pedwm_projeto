using System;
using System.Threading.Tasks;
using ApplicationLayer.Commands;
using ApplicationLayer.Repositories;
using ApplicationLayer.Services;
using DomainLayer.Domain.Projects;
using DomainLayer.Domain.Builders;
using Moq;
using FluentAssertions;
using Xunit;

namespace ApplicationLayer.Tests.Services
{
    public class ProjectServiceTests
    {
        private readonly Mock<IProjectRepository> _projectRepositoryMock;
        private readonly Mock<IAppLogger> _loggerMock;
        private readonly ProjectService _sut;

        public ProjectServiceTests()
        {
            _projectRepositoryMock = new Mock<IProjectRepository>();
            _loggerMock = new Mock<IAppLogger>();

            // Injetamos as falsificações no serviço
            _sut = new ProjectService(_projectRepositoryMock.Object, _loggerMock.Object);
        }

        // ==========================================
        // TESTE 1: Horas Negativas
        // ==========================================
        [Fact]
        public async Task AddConsumedHours_ShouldThrowArgumentException_WhenHoursAreNegative()
        {
            var command = new AddHoursToProjectCommand
            {
                ProjectId = Guid.NewGuid().ToString(),
                Hours = -5 // Valor inválido!
            };

            Func<Task> act = async () => await _sut.AddConsumedHoursToProjectAsync(command);

            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("As horas devem ser um valor positivo.");
        }

        // ==========================================
        // TESTE 2: ID Inválido
        // ==========================================
        [Fact]
        public async Task AddConsumedHours_ShouldThrowArgumentException_WhenIdIsNotGuid()
        {
            var command = new AddHoursToProjectCommand
            {
                ProjectId = "isto-nao-e-um-guid", // Formato errado
                Hours = 10
            };

            Func<Task> act = async () => await _sut.AddConsumedHoursToProjectAsync(command);

            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("ID do projeto inválido.");
        }

        // ==========================================
        // TESTE 3: Sucesso
        // ==========================================
        [Fact]
        public async Task AddConsumedHours_ShouldAddHoursAndSave_WhenValid()
        {
            var projectId = Guid.NewGuid();
            var command = new AddHoursToProjectCommand
            {
                ProjectId = projectId.ToString(),
                Hours = 8
            };

            var fakeProject = new ProjectBuilder()
                .WithId(projectId)
                .WithTitle("Novo Website")
                .WithDates(DateTime.Now, DateTime.Now.AddDays(10))
                .ManagedBy(Guid.NewGuid())
                .Build() as Project; // Garantimos que é do tipo correto

            _projectRepositoryMock.Setup(repo => repo.GetByIdAsync(projectId)).ReturnsAsync(fakeProject);

            var result = await _sut.AddConsumedHoursToProjectAsync(command);

            result.Should().NotBeNull();

            // Garantir que o repositório mandou gravar as alterações
            _projectRepositoryMock.Verify(repo => repo.SaveAsync(It.IsAny<Project>()), Times.Once);

            // Garantir que o logger registou o sucesso
            _loggerMock.Verify(l => l.LogInfo(It.Is<string>(msg => msg.Contains("Registadas 8 horas"))), Times.Once);
        }

        // ==========================================
        // TESTE 4: Mudar Estado - ID Inválido
        // ==========================================
        [Fact]
        public async Task ChangeProjectStatus_ShouldThrowArgumentException_WhenIdIsNotGuid()
        {
            var command = new ChangeProjectStatusCommand
            {
                ProjectId = "texto-invalido",
                Status = ProjectStatus.Completed
            };

            Func<Task> act = async () => await _sut.ChangeProjectStatusAsync(command);

            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("ID do projeto inválido.");
        }

        // ==========================================
        // TESTE 5: Mudar Estado - Projeto Não É Standard ou Null
        // ==========================================
        [Fact]
        public async Task ChangeProjectStatus_ShouldThrowInvalidOperationException_WhenProjectIsNotStandard()
        {
            var projectId = Guid.NewGuid();
            var command = new ChangeProjectStatusCommand
            {
                ProjectId = projectId.ToString(),
                Status = ProjectStatus.Completed
            };

            // O Mock a devolve NULL (não encontrou o projeto)
            _projectRepositoryMock.Setup(repo => repo.GetByIdAsync(projectId)).ReturnsAsync((ProjectBase)null);

            Func<Task> act = async () => await _sut.ChangeProjectStatusAsync(command);

            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Apenas projetos standard suportam alteração de estado.");
        }

        // ==========================================
        // TESTE 6: Mudar Estado - Sucesso
        // ==========================================
        [Fact]
        public async Task ChangeProjectStatus_ShouldChangeStatusAndSave_WhenValid()
        {
            var projectId = Guid.NewGuid();
            var command = new ChangeProjectStatusCommand
            {
                ProjectId = projectId.ToString(),
                Status = ProjectStatus.Completed
            };

            var fakeProject = new ProjectBuilder()
                .WithId(projectId)
                .WithTitle("Migração Cloud")
                .WithDates(DateTime.Now, DateTime.Now.AddDays(15))
                .ManagedBy(Guid.NewGuid())
                .Build();

            _projectRepositoryMock.Setup(repo => repo.GetByIdAsync(projectId)).ReturnsAsync(fakeProject);

            var result = await _sut.ChangeProjectStatusAsync(command);

            result.Should().NotBeNull();

            // Garantir que o repositório foi chamado para guardar
            _projectRepositoryMock.Verify(repo => repo.SaveAsync(It.IsAny<Project>()), Times.Once);

            // Garantir que o log registou a alteração de estado
            _loggerMock.Verify(l => l.LogInfo(It.Is<string>(msg => msg.Contains("alterado para"))), Times.Once);
        }
    }
}