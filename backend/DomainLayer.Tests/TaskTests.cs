using System;
using Xunit;
using FluentAssertions;
using DomainLayer.Domain.Tasks;
using DomainLayer.Domain.Builders;

namespace DomainLayer.Tests.Domain
{
    public class TaskTests
    {
        private static Guid AnyProject() => Guid.NewGuid();

        [Fact]
        public void FeatureTask_ShouldBeCreatedWithCorrectTaskType()
        {
            var task = new FeatureTaskBuilder()
                .InProject(AnyProject())
                .WithTitle("Desenvolver Login")
                .Build();

            task.Should().BeOfType<FeatureTask>();
            task.Title.Should().Be("Desenvolver Login");
        }

        [Fact]
        public void BugTask_ShouldBeCreatedWithCorrectTaskType()
        {
            var task = new BugTaskBuilder()
                .InProject(AnyProject())
                .WithTitle("Erro no Logout")
                .Build();

            task.Should().BeOfType<BugTask>();
        }

        [Fact]
        public void UpdateAssignedUser_ShouldChangeUserCorrectly()
        {
            var task = new FeatureTaskBuilder()
                .InProject(AnyProject())
                .WithTitle("Tarefa")
                .Build();
            var newUser = Guid.NewGuid();

            task.ChangeAssignee(newUser);

            task.AssignedUserId.Should().Be(newUser);
        }

        [Fact]
        public void MarkAsCompleted_ShouldUpdateStatusAndDate()
        {
            var task = new FeatureTaskBuilder()
                .InProject(AnyProject())
                .WithTitle("T")
                .Build();

            task.MarkAsCompleted();

            task.Status.Should().Be(DomainLayer.Domain.Tasks.TaskStatus.Completed);
            task.CompletedAt.Should().NotBeNull();
        }
    }
}