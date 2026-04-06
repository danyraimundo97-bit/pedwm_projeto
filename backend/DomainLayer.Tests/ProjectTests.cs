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
                .Build();

            project.Status.Should().Be(ProjectStatus.Active);
            project.Title.Should().Be("Sistema de Gestão");
        }

        [Fact]
        public void UpdateStatus_ShouldChangeProjectStatus_WhenValidStatusIsProvided()
        {
            var project = new ProjectBuilder().WithTitle("Projeto Teste").Build();

            project.ChangeStatus(ProjectStatus.Completed);

            project.Status.Should().Be(ProjectStatus.Completed);
        }

        [Fact]
        public void Project_ShouldThrowException_WhenTitleIsEmpty()
        {
            Action act = () => new ProjectBuilder()
                .WithTitle("") // Título inválido
                .Build();

            act.Should().Throw<ArgumentException>()
                .WithMessage("*título*");
        }

        [Fact]
        public void Project_ShouldThrowException_WhenDatesAreInverted()
        {
            var startDate = DateTime.Now.AddDays(10);
            var endDate = DateTime.Now; // Data de fim antes do início!

            Action act = () => new ProjectBuilder()
                .WithTitle("Projeto Impossível")
                .WithDates(startDate, endDate)
                .Build();

            act.Should().Throw<ArgumentException>()
                .WithMessage("*data*");
        }

        [Fact]
        public void Project_ShouldNotAllowEmptyTitle()
        {
            Action act = () => new ProjectBuilder().WithTitle("").Build();

            act.Should().Throw<ArgumentException>();
        }
    }
}