using System;
using System.Threading.Tasks;
using ApplicationLayer.Commands;
using ApplicationLayer.Repositories;
using ApplicationLayer.Services;
using DomainLayer.Domain.Teams;
using DomainLayer.Domain.Builders;
using Moq;
using FluentAssertions;
using Xunit;

namespace ApplicationLayer.Tests.Services
{
    public class TeamServiceTests
    {
        private readonly Mock<ITeamRepository> _teamRepositoryMock;
        private readonly Mock<IAppLogger> _loggerMock;
        private readonly TeamService _sut;

        public TeamServiceTests()
        {
            _teamRepositoryMock = new Mock<ITeamRepository>();
            _loggerMock = new Mock<IAppLogger>();

            // Injetamos as dependências falsas
            _sut = new TeamService(_teamRepositoryMock.Object, _loggerMock.Object);
        }

        // ==========================================
        // TESTE 1: Nome da equipa vazio
        // ==========================================
        [Fact]
        public async Task CreateTeam_ShouldThrowArgumentException_WhenNameIsEmpty()
        {
            var command = new CreateTeamCommand { Name = "" };

            Func<Task> act = async () => await _sut.CreateTeamAsync(command);

            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("O nome da equipa é obrigatório.");
        }

        // ==========================================
        // TESTE 2: Sucesso
        // ==========================================
        [Fact]
        public async Task CreateTeam_ShouldCreateAndSave_WhenValid()
        {
            var command = new CreateTeamCommand { Name = "Backend Devs" };

            var result = await _sut.CreateTeamAsync(command);

            result.Should().NotBeNull();
            result.Name.Should().Be("Backend Devs");

            // Garantimos que o serviço chamou o repositório para guardar a nova equipa
            _teamRepositoryMock.Verify(repo => repo.SaveAsync(It.IsAny<Team>()), Times.Once);

            // Garantimos que o registo foi parar ao log
            _loggerMock.Verify(l => l.LogInfo(It.IsAny<string>()), Times.Once);
        }
    }
}