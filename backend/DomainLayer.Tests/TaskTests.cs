using System;
using Xunit;
using FluentAssertions;
using DomainLayer.Domain.Tasks;
using DomainLayer.Domain.Builders;

namespace DomainLayer.Tests.Domain
{
    public class TaskTests
    {
        [Fact]
        public void FeatureTask_ShouldBeCreatedWithCorrectTaskType()
        {
            var task = (FeatureTask)new FeatureTaskBuilder()
                .WithTitle("Desenvolver Login")
                .Build();

            task.Type.Should().Be(TaskType.Feature);
            task.Title.Should().Be("Desenvolver Login");
        }

        [Fact]
        public void BugTask_ShouldBeCreatedWithCorrectTaskType()
        {
            var task = (BugTask)new BugTaskBuilder()
                .WithTitle("Erro no Logout")
                .Build();

            task.Type.Should().Be(TaskType.Bug);
        }

        [Fact]
        public void UpdateAssignedUser_ShouldChangeUserCorrectly()
        {
            var task = new FeatureTaskBuilder().WithTitle("Tarefa").Build();
            var newUser = Guid.NewGuid();

            task.ChangeAssignee(newUser);

            task.AssignedUserId.Should().Be(newUser);
        }

        [Fact]
        public void MarkAsCompleted_ShouldUpdateStatusAndDate()
        {
            var task = new FeatureTaskBuilder().Build();

            task.MarkAsCompleted();

            task.Status.Should().Be(DomainLayer.Domain.Tasks.TaskStatus.Completed);
            task.CompletedAt.Should().NotBeNull();
        }
    }
}