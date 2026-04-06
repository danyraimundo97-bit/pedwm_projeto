using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

using DomainLayer.Domain.Builders;
using DomainLayer.Domain.Projects;
using DomainLayer.Domain.Tasks;
using InfrastructureLayer.Data;
using InfrastructureLayer.Repositories;

namespace InfrastructureLayer.Tests.Repositories
{
    public class ProjectRepositoryTests
    {
        // ==========================================
        // SETUP: Cria uma BD limpa e isolada por cada teste
        // ==========================================
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Garante paralelismo seguro no xUnit
                .Options;

            return new AppDbContext(options);
        }

        // ==========================================
        // TESTES DO MÉTODO: SaveAsync
        // ==========================================

        [Fact]
        public async Task SaveAsync_ShouldInsertProject_WhenProjectDoesNotExist()
        {
            var dbContext = GetDbContext();
            var repository = new ProjectRepository(dbContext);

            var projectId = Guid.NewGuid();
            var project = new ProjectBuilder()
                .WithId(projectId)
                .WithTitle("Projeto Novo")
                .WithDates(DateTime.Now, DateTime.Now.AddDays(10))
                .ManagedBy(Guid.NewGuid())
                .Build();

            await repository.SaveAsync(project);

            var savedProject = await dbContext.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
            savedProject.Should().NotBeNull();
            savedProject.Title.Should().Be("Projeto Novo");
            dbContext.Projects.Should().HaveCount(1);
        }

        [Fact]
        public async Task SaveAsync_ShouldUpdateProject_WhenProjectAlreadyExists()
        {
            var dbContext = GetDbContext();
            var repository = new ProjectRepository(dbContext);

            var projectId = Guid.NewGuid();
            var originalProject = new ProjectBuilder()
                .WithId(projectId)
                .WithTitle("Título Original")
                .WithDates(DateTime.Now, DateTime.Now.AddDays(10))
                .ManagedBy(Guid.NewGuid())
                .Build();

            // Inserir o projeto manualmente na BD
            dbContext.Projects.Add(originalProject);
            await dbContext.SaveChangesAsync();

            // Simular uma chamada isolada de atualização
            dbContext.ChangeTracker.Clear();

            // Criamos uma cópia do projeto com o mesmo ID, mas título alterado
            var updatedProject = new ProjectBuilder()
                .WithId(projectId)
                .WithTitle("Título Atualizado")
                .WithDates(DateTime.Now, DateTime.Now.AddDays(10))
                .ManagedBy(Guid.NewGuid())
                .Build();

            await repository.SaveAsync(updatedProject);

            var result = await dbContext.Projects.AsNoTracking().FirstOrDefaultAsync(p => p.Id == projectId);
            result.Should().NotBeNull();
            result.Title.Should().Be("Título Atualizado");
            dbContext.Projects.Should().HaveCount(1); // Não pode ter inserido um novo, tem de continuar a ser 1
        }

        // ==========================================
        // TESTES DO MÉTODO: GetByIdAsync
        // ==========================================

        [Fact]
        public async Task GetByIdAsync_ShouldReturnProject_WhenExists()
        {
            var dbContext = GetDbContext();
            var repository = new ProjectRepository(dbContext);

            var projectId = Guid.NewGuid();
            var project = new ProjectBuilder().WithId(projectId).WithTitle("Projeto XPTO").Build();

            dbContext.Projects.Add(project);
            await dbContext.SaveChangesAsync();

            var result = await repository.GetByIdAsync(projectId);

            result.Should().NotBeNull();
            result.Id.Should().Be(projectId);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenNotExists()
        {
            var dbContext = GetDbContext();
            var repository = new ProjectRepository(dbContext);

            var result = await repository.GetByIdAsync(Guid.NewGuid());

            result.Should().BeNull();
        }

        // ==========================================
        // TESTES DO MÉTODO: GetPagedAsync
        // ==========================================

        [Fact]
        public async Task GetPagedAsync_ShouldReturnPaginatedAndOrderedByStartDate()
        {
            var dbContext = GetDbContext();
            var repository = new ProjectRepository(dbContext);

            var today = DateTime.Today;

            // Inserir 3 projetos com datas de início diferentes
            dbContext.Projects.AddRange(
                new ProjectBuilder().WithId(Guid.NewGuid()).WithTitle("Mais Recente").WithDates(today.AddDays(5), today.AddDays(10)).Build(),
                new ProjectBuilder().WithId(Guid.NewGuid()).WithTitle("Mais Antigo").WithDates(today.AddDays(-5), today.AddDays(10)).Build(),
                new ProjectBuilder().WithId(Guid.NewGuid()).WithTitle("Intermédio").WithDates(today, today.AddDays(10)).Build()
            );
            await dbContext.SaveChangesAsync();

            // Pedimos a página 1, com tamanho 2 (só deve devolver os 2 mais antigos devido ao OrderBy)
            var result = await repository.GetPagedAsync(page: 1, size: 2);

            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result[0].Title.Should().Be("Mais Antigo");
            result[1].Title.Should().Be("Intermédio");
        }

        // ==========================================
        // TESTES DO MÉTODO: GetByUserAsync
        // ==========================================

        [Fact]
        public async Task GetByUserAsync_ShouldReturnOnlyProjects_WhereUserHasTasks()
        {
            var dbContext = GetDbContext();
            var repository = new ProjectRepository(dbContext);

            var targetUserId = Guid.NewGuid();
            var otherUserId = Guid.NewGuid();

            var project1Id = Guid.NewGuid();
            var project2Id = Guid.NewGuid();
            var project3Id = Guid.NewGuid(); // Projeto sem tarefas do user

            // Criar os Projetos
            dbContext.Projects.AddRange(
                new ProjectBuilder().WithId(project1Id).WithTitle("Proj 1").Build(),
                new ProjectBuilder().WithId(project2Id).WithTitle("Proj 2").Build(),
                new ProjectBuilder().WithId(project3Id).WithTitle("Proj 3").Build()
            );

            // Criar as Tarefas associadas aos projetos e utilizadores
            var task1 = new FeatureTaskBuilder().WithId(Guid.NewGuid()).Build();
            SetPrivateProperty(task1, "AssignedUserId", targetUserId);
            SetPrivateProperty(task1, "ProjectId", project1Id);

            var task2 = new FeatureTaskBuilder().WithId(Guid.NewGuid()).Build();
            SetPrivateProperty(task2, "AssignedUserId", targetUserId);
            SetPrivateProperty(task2, "ProjectId", project2Id);

            var task3 = new FeatureTaskBuilder().WithId(Guid.NewGuid()).Build();
            SetPrivateProperty(task3, "AssignedUserId", otherUserId); // Tarefa de outro user
            SetPrivateProperty(task3, "ProjectId", project3Id);

            dbContext.Tasks.AddRange(task1, task2, task3);
            await dbContext.SaveChangesAsync();

            var result = await repository.GetByUserAsync(targetUserId);

            result.Should().NotBeNull();
            result.Should().HaveCount(2); // Deve trazer o Proj 1 e Proj 2
            result.Select(p => p.Id).Should().Contain(new[] { project1Id, project2Id });
            result.Select(p => p.Id).Should().NotContain(project3Id);
        }

        [Fact]
        public async Task GetByUserAsync_ShouldReturnEmptyList_WhenUserHasNoTasks()
        {
            var dbContext = GetDbContext();
            var repository = new ProjectRepository(dbContext);

            var project = new ProjectBuilder().WithId(Guid.NewGuid()).Build();
            dbContext.Projects.Add(project);
            await dbContext.SaveChangesAsync();

            var result = await repository.GetByUserAsync(Guid.NewGuid()); // Passamos um User ID fantasma

            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        // --- Método Auxiliar ---
        private void SetPrivateProperty(object instance, string propertyName, object value)
        {
            var property = instance.GetType().GetProperty(propertyName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (property != null)
            {
                property.SetValue(instance, value);
            }
        }
    }
}