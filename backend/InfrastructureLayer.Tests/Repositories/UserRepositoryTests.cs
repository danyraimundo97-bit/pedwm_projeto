using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

using DomainLayer.Domain.Builders;
using DomainLayer.Domain.Users;
using InfrastructureLayer.Data;
using InfrastructureLayer.Repositories;

namespace InfrastructureLayer.Tests.Repositories
{
    public class UserRepositoryTests
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
        public async Task SaveAsync_ShouldInsertUser_WhenUserDoesNotExist()
        {
            var dbContext = GetDbContext();
            var repository = new UserRepository(dbContext);

            var userId = Guid.NewGuid();
            var user = new UserBuilder()
                .WithId(userId)
                .WithName("João Silva")
                .WithEmail("joao.silva@empresa.com")
                .Build();

            await repository.SaveAsync(user);

            var savedUser = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            savedUser.Should().NotBeNull();
            savedUser.Name.Should().Be("João Silva");
            savedUser.Email.Should().Be("joao.silva@empresa.com");
            dbContext.Users.Should().HaveCount(1);
        }

        [Fact]
        public async Task SaveAsync_ShouldUpdateUser_WhenUserAlreadyExists()
        {
            var dbContext = GetDbContext();
            var repository = new UserRepository(dbContext);

            var userId = Guid.NewGuid();
            var originalUser = new UserBuilder()
                .WithId(userId)
                .WithName("Nome Original")
                .WithEmail("original@empresa.com")
                .Build();

            dbContext.Users.Add(originalUser);
            await dbContext.SaveChangesAsync();

            // Limpar o tracker para simular novo pedido
            dbContext.ChangeTracker.Clear();

            var updatedUser = new UserBuilder()
                .WithId(userId)
                .WithName("Nome Atualizado")
                .WithEmail("atualizado@empresa.com")
                .Build();

            await repository.SaveAsync(updatedUser);

            var result = await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);
            result.Should().NotBeNull();
            result.Name.Should().Be("Nome Atualizado");
            result.Email.Should().Be("atualizado@empresa.com");
            dbContext.Users.Should().HaveCount(1); // Garante que foi Update e não Insert
        }

        // ==========================================
        // TESTES DO MÉTODO: GetByIdAsync
        // ==========================================

        [Fact]
        public async Task GetByIdAsync_ShouldReturnUser_WhenExists()
        {
            var dbContext = GetDbContext();
            var repository = new UserRepository(dbContext);

            var userId = Guid.NewGuid();
            var user = new UserBuilder()
                .WithId(userId)
                .WithName("Ana Costa")
                .Build();

            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();

            var result = await repository.GetByIdAsync(userId);

            result.Should().NotBeNull();
            result.Id.Should().Be(userId);
            result.Name.Should().Be("Ana Costa");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenNotExists()
        {
            var dbContext = GetDbContext();
            var repository = new UserRepository(dbContext);

            var result = await repository.GetByIdAsync(Guid.NewGuid());

            result.Should().BeNull();
        }

        // ==========================================
        // TESTES DO MÉTODO: GetPagedAsync
        // ==========================================

        [Fact]
        public async Task GetPagedAsync_ShouldReturnPaginatedAndOrderedByName()
        {
            var dbContext = GetDbContext();
            var repository = new UserRepository(dbContext);

            // Inserimos 3 utilizadores desordenados alfabeticamente
            var user1 = new UserBuilder().WithId(Guid.NewGuid()).WithName("Zacarias").Build();
            var user2 = new UserBuilder().WithId(Guid.NewGuid()).WithName("Alice").Build();
            var user3 = new UserBuilder().WithId(Guid.NewGuid()).WithName("Bruno").Build();

            dbContext.Users.AddRange(user1, user2, user3);
            await dbContext.SaveChangesAsync();

            // Pedimos a página 1, tamanho 2.
            // Query faz OrderBy(Name), os esperados são "Alice" e "Bruno".
            var result = await repository.GetPagedAsync(page: 1, size: 2);

            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result[0].Name.Should().Be("Alice");
            result[1].Name.Should().Be("Bruno");
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