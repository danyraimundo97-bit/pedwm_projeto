using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

using DomainLayer.Domain.Builders;
using DomainLayer.Domain.Tasks;
using InfrastructureLayer.Data;
using InfrastructureLayer.Repositories;

namespace InfrastructureLayer.Tests.Repositories
{
    public class TaskRepositoryTests
    {
        // ==========================================
        // SETUP: Cria uma BD limpa e isolada por cada teste
        // ==========================================
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        // ==========================================
        // TESTES DO MÉTODO: SaveAsync
        // ==========================================

        [Fact]
        public async Task SaveAsync_ShouldInsertTask_WhenTaskDoesNotExist()
        {
            var dbContext = GetDbContext();
            var repository = new TaskRepository(dbContext);

            var taskId = Guid.NewGuid();
            var task = new FeatureTaskBuilder()
                .WithId(taskId)
                .WithTitle("Nova Tarefa")
                .Build();

            await repository.SaveAsync(task);

            var savedTask = await dbContext.Tasks.FirstOrDefaultAsync(t => t.Id == taskId);
            savedTask.Should().NotBeNull();
            savedTask.Title.Should().Be("Nova Tarefa");
            dbContext.Tasks.Should().HaveCount(1);
        }

        [Fact]
        public async Task SaveAsync_ShouldUpdateTask_WhenTaskAlreadyExists()
        {
            var dbContext = GetDbContext();
            var repository = new TaskRepository(dbContext);

            var taskId = Guid.NewGuid();
            var originalTask = new FeatureTaskBuilder()
                .WithId(taskId)
                .WithTitle("Título Antigo")
                .Build();

            dbContext.Tasks.Add(originalTask);
            await dbContext.SaveChangesAsync();

            // Limpa o tracker para simular novo pedido
            dbContext.ChangeTracker.Clear();

            var updatedTask = new FeatureTaskBuilder()
                .WithId(taskId)
                .WithTitle("Título Atualizado")
                .Build();

            await repository.SaveAsync(updatedTask);

            var result = await dbContext.Tasks.AsNoTracking().FirstOrDefaultAsync(t => t.Id == taskId);
            result.Should().NotBeNull();
            result.Title.Should().Be("Título Atualizado");
            dbContext.Tasks.Should().HaveCount(1); // Garante que foi Update e não Insert
        }

        // ==========================================
        // TESTES DO MÉTODO: GetPagedAsync
        // ==========================================

        [Fact]
        public async Task GetPagedAsync_ShouldReturnPaginatedAndOrderedById()
        {
            var dbContext = GetDbContext();
            var repository = new TaskRepository(dbContext);

            // Inserir 3 tarefas (ordem por ID gerado)
            var task1 = new FeatureTaskBuilder().WithTitle("Task A").Build();
            var task2 = new FeatureTaskBuilder().WithTitle("Task B").Build();
            var task3 = new FeatureTaskBuilder().WithTitle("Task C").Build();

            dbContext.Tasks.AddRange(task1, task2, task3);
            await dbContext.SaveChangesAsync();

            // Obter a lista em memória ordenada por ID
            var expectedTasks = new[] { task1, task2, task3 }.OrderBy(t => t.Id).ToList();

            var result = await repository.GetPagedAsync(page: 1, size: 2);

            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result[0].Id.Should().Be(expectedTasks[0].Id);
            result[1].Id.Should().Be(expectedTasks[1].Id);
        }

        // ==========================================
        // TESTES DOS MÉTODOS: GetByIdAsync & GetTaskAsync (String)
        // ==========================================

        [Fact]
        public async Task GetByIdAsync_ShouldReturnTask_WhenExists()
        {
            var dbContext = GetDbContext();
            var repository = new TaskRepository(dbContext);

            var taskId = Guid.NewGuid();
            var task = new FeatureTaskBuilder().WithId(taskId).Build();

            dbContext.Tasks.Add(task);
            await dbContext.SaveChangesAsync();

            var result = await repository.GetByIdAsync(taskId);

            result.Should().NotBeNull();
            result.Id.Should().Be(taskId);
        }

        [Fact]
        public async Task GetTaskAsync_WithStrings_ShouldReturnTask_WhenIdsAreValid()
        {
            var dbContext = GetDbContext();
            var repository = new TaskRepository(dbContext);

            var taskId = Guid.NewGuid();
            var projectId = Guid.NewGuid();

            var task = new FeatureTaskBuilder().WithId(taskId).Build();
            SetPrivateProperty(task, "ProjectId", projectId); // Associar ao projeto

            dbContext.Tasks.Add(task);
            await dbContext.SaveChangesAsync();

            // Usando os métodos com String
            var result = await repository.GetTaskAsync(taskId.ToString(), projectId.ToString());

            result.Should().NotBeNull();
            result.Id.Should().Be(taskId);
            result.ProjectId.Should().Be(projectId);
        }

        [Fact]
        public async Task GetTaskAsync_WithStrings_ShouldReturnNull_WhenStringIsInvalid()
        {
            var dbContext = GetDbContext();
            var repository = new TaskRepository(dbContext);

            // Passar strings que não são GUIDs válidos
            var result = await repository.GetTaskAsync("id-invalido", Guid.NewGuid().ToString());

            // O TryParse deve falhar e devolver nulo
            result.Should().BeNull();
        }

        // ==========================================
        // TESTES DOS MÉTODOS: GetByProject & GetByUser
        // ==========================================

        [Fact]
        public async Task GetByProjectAsync_ShouldReturnOnlyTasksForSpecificProject()
        {
            var dbContext = GetDbContext();
            var repository = new TaskRepository(dbContext);

            var targetProjectId = Guid.NewGuid();
            var otherProjectId = Guid.NewGuid();

            var task1 = new FeatureTaskBuilder().WithId(Guid.NewGuid()).Build();
            SetPrivateProperty(task1, "ProjectId", targetProjectId);

            var task2 = new FeatureTaskBuilder().WithId(Guid.NewGuid()).Build();
            SetPrivateProperty(task2, "ProjectId", otherProjectId);

            dbContext.Tasks.AddRange(task1, task2);
            await dbContext.SaveChangesAsync();

            var result = await repository.GetByProjectAsync(targetProjectId);

            result.Should().HaveCount(1);
            result.First().Id.Should().Be(task1.Id);
        }

        [Fact]
        public async Task GetByUserAsync_ShouldReturnOnlyTasksForSpecificUser()
        {
            var dbContext = GetDbContext();
            var repository = new TaskRepository(dbContext);

            var targetUserId = Guid.NewGuid();
            var otherUserId = Guid.NewGuid();

            var task1 = new FeatureTaskBuilder().WithId(Guid.NewGuid()).Build();
            SetPrivateProperty(task1, "AssignedUserId", targetUserId);

            var task2 = new FeatureTaskBuilder().WithId(Guid.NewGuid()).Build();
            SetPrivateProperty(task2, "AssignedUserId", otherUserId);

            dbContext.Tasks.AddRange(task1, task2);
            await dbContext.SaveChangesAsync();

            var result = await repository.GetByUserAsync(targetUserId);

            result.Should().HaveCount(1);
            result.First().Id.Should().Be(task1.Id);
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