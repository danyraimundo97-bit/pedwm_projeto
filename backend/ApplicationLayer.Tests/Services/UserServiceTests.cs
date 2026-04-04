using ApplicationLayer.Commands;
using ApplicationLayer.Repositories;
using ApplicationLayer.Services;
using DomainLayer.Domain.Builders;
using DomainLayer.Domain.Teams;
using DomainLayer.Domain.Users;
using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ApplicationLayer.Tests.Services
{
    public class UserServiceTests
    {
        // Variáveis para os "Mocks"
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<ITeamRepository> _teamRepositoryMock;
        private readonly Mock<IAppLogger> _loggerMock;

        // A classe que vamos testar
        private readonly UserService _sut; // SUT = System Under Test

        public UserServiceTests()
        {
            // Instanciamos os Mocks
            _userRepositoryMock = new Mock<IUserRepository>();
            _teamRepositoryMock = new Mock<ITeamRepository>();
            _loggerMock = new Mock<IAppLogger>();

            // Injetamos os falsos no serviço verdadeiro
            _sut = new UserService(
                _userRepositoryMock.Object,
                _teamRepositoryMock.Object,
                _loggerMock.Object);
        }

        // ==========================================
        // TESTE 1: Regra de Negócio Falha
        // ==========================================
        [Fact]
        public async Task AssignUserToTeam_ShouldThrowArgumentException_WhenIdsAreEmpty()
        {
            var command = new AssignUserToTeamCommand
            {
                UserId = "", // ID Vazio para forçar o erro!
                TeamId = "algum-id"
            };

            Func<Task> act = async () => await _sut.AssignUserToTeamAsync(command);

            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("O identificador do utilizador e da equipa são obrigatórios.");

            // Verifica se o logger foi chamado para registar o aviso
            _loggerMock.Verify(l => l.LogWarning(It.IsAny<string>()), Times.Once);
        }

        // ==========================================
        // TESTE 2: Sucesso
        // ==========================================
        [Fact]
        public async Task AssignUserToTeam_ShouldAssignTeam_WhenValid()
        {
            var userId = Guid.NewGuid();
            var teamId = Guid.NewGuid();
            var command = new AssignUserToTeamCommand { UserId = userId.ToString(), TeamId = teamId.ToString() };

            var fakeUser = new UserBuilder()
                .WithId(userId)
                .WithName("João")
                //.WithEmail("joao@teste.com")
                .Build();

            var fakeTeam = new TeamBuilder()
                .WithId(teamId)
                .WithName("Frontend")
                .Build();

            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(fakeUser);

            _teamRepositoryMock.Setup(repo => repo.GetByIdAsync(teamId)).ReturnsAsync(fakeTeam);

            var result = await _sut.AssignUserToTeamAsync(command);

            result.Should().NotBeNull();
            result.TeamId.Should().Be(teamId); // O utilizador tem de ter o novo TeamId!

            // Garantir que o repositório chamou o SaveAsync exatament 1 vez
            _userRepositoryMock.Verify(repo => repo.SaveAsync(It.IsAny<User>()), Times.Once);
        }
    }
}