using System;
using Xunit;
using FluentAssertions;
using DomainLayer.Domain.Projects;
using DomainLayer.Domain.Builders;

namespace DomainLayer.Tests.Domain
{
    public class ProjectTests
    {
        [Fact]
        public void Project_ShouldBeCreatedWithDefaultStatusActive()
        {
            var project = new ProjectBuilder()
                .WithId(Guid.NewGuid())
                .WithTitle("Sistema de Gestão")
                .WithDates(DateTime.Now, DateTime.Now.AddDays(10))
                .ManagedBy(Guid.NewGuid())
                .Build();

            project.Status.Should().Be(ProjectStatus.Active);
            project.Title.Should().Be("Sistema de Gestão");
        }

        [Fact]
        public void UpdateStatus_ShouldChangeProjectStatus_WhenValidStatusIsProvided()
        {
            var project = new ProjectBuilder()
                .WithTitle("Projeto Teste")
                .WithDates(DateTime.UtcNow, DateTime.UtcNow.AddDays(1))
                .ManagedBy(Guid.NewGuid())
                .Build();

            project.ChangeStatus(ProjectStatus.Completed);

            project.Status.Should().Be(ProjectStatus.Completed);
        }

        [Fact]
        public void Project_ShouldThrowException_WhenTitleIsEmpty()
        {
            Action act = () => new ProjectBuilder()
                .WithTitle("") // Título inválido
                .ManagedBy(Guid.NewGuid())
                .Build();

            act.Should().Throw<InvalidOperationException>()
                .WithMessage("*WithTitle*");
        }

        [Fact]
        public void Project_ShouldThrowException_WhenDatesAreInverted()
        {
            var startDate = DateTime.Now.AddDays(10);
            var endDate = DateTime.Now; // Data de fim antes do início!

            Action act = () => new ProjectBuilder()
                .WithTitle("Projeto Impossível")
                .WithDates(startDate, endDate)
                .ManagedBy(Guid.NewGuid())
                .Build();

            act.Should().Throw<InvalidOperationException>()
                .WithMessage("*End date*");
        }

        [Fact]
        public void Project_ShouldNotAllowEmptyTitle()
        {
            Action act = () => new ProjectBuilder()
                .WithTitle("")
                .ManagedBy(Guid.NewGuid())
                .Build();

            act.Should().Throw<InvalidOperationException>();
        }
    }
}