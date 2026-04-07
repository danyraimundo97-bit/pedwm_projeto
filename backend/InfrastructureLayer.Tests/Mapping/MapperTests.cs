using System;
using Xunit;
using FluentAssertions;
using InfrastructureLayer.Mapping;
using DomainLayer.Domain.Projects;
using DomainLayer.Domain.Tasks;
using DomainLayer.Domain.Users;
using DomainLayer.Domain.Teams;
using DomainLayer.Domain.Builders;

namespace InfrastructureLayer.Tests.Mapping
{
    public class MapperTests
    {
        private readonly DomainEntityDtoMapper _sut;

        public MapperTests()
        {
            // Inicializa o Mapster
            MapsterConfiguration.Register();
            _sut = new DomainEntityDtoMapper();
        }

        [Fact]
        public void ToProjectResponse_ShouldMapAllFieldsCorrectly()
        {
            var projectId = Guid.NewGuid();
            var managerId = Guid.NewGuid();
            var project = new ProjectBuilder()
                .WithId(projectId)
                .WithTitle("Sistema de Testes")
                .WithDates(DateTime.Now, DateTime.Now.AddDays(30))
                .ManagedBy(managerId)
                .Build();

            var result = _sut.ToProjectResponse(project);

            result.Should().NotBeNull();
            result.Id.Should().Be(project.Id);
            result.Title.Should().Be("Sistema de Testes");
        }

        [Fact]
        public async System.Threading.Tasks.Task ToTaskResponse_ShouldMapTaskProperties()
        {
            var taskId = Guid.NewGuid();
            var task = new FeatureTaskBuilder()
                .InProject(Guid.NewGuid())
                .WithId(taskId)
                .WithTitle("Desenvolver API")
                .Build();

            var result = _sut.ToTaskResponse(task);

            result.Should().NotBeNull();
            result.Id.Should().Be(taskId);
            result.Title.Should().Be("Desenvolver API");
        }

        [Fact]
        public void ToUserResponse_ShouldMapUserToUserResponse()
        {
            var userId = Guid.NewGuid();
            var user = new UserBuilder()
                .WithId(userId)
                .WithName("Tiago")
                .WithEmail("tiago@teste.com")
                .Build();

            var result = _sut.ToUserResponse(user);

            result.Should().NotBeNull();
            result.Id.Should().Be(userId);
            result.Name.Should().Be("Tiago");
            result.Email.Should().Be("tiago@teste.com");
        }

        [Fact]
        public void ToTeamResponse_ShouldMapTeamCorrectly()
        {
            var teamId = Guid.NewGuid();
            var team = new TeamBuilder()
                .WithId(teamId)
                .WithName("Backend Devs")
                .Build();

            var result = _sut.ToTeamResponse(team);

            result.Should().NotBeNull();
            result.Id.Should().Be(teamId);
            result.Name.Should().Be("Backend Devs");
        }
    }
}