using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

using DomainLayer.Domain.Builders;
using DomainLayer.Domain.Teams;
using InfrastructureLayer.Data;
using InfrastructureLayer.Repositories;

namespace InfrastructureLayer.Tests.Repositories
{
    public class TeamRepositoryTests
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
        public async Task SaveAsync_ShouldInsertTeam_WhenTeamDoesNotExist()
        {
            var dbContext = GetDbContext();
            var repository = new TeamRepository(dbContext);

            var teamId = Guid.NewGuid();
            var team = new TeamBuilder()
                .WithId(teamId)
                .WithName("Equipa Backend")
                .Build();

            await repository.SaveAsync(team);

            var savedTeam = await dbContext.Teams.FirstOrDefaultAsync(t => t.Id == teamId);
            savedTeam.Should().NotBeNull();
            savedTeam.Name.Should().Be("Equipa Backend");
            dbContext.Teams.Should().HaveCount(1);
        }

        [Fact]
        public async Task SaveAsync_ShouldUpdateTeam_WhenTeamAlreadyExists()
        {
            var dbContext = GetDbContext();
            var repository = new TeamRepository(dbContext);

            var teamId = Guid.NewGuid();
            var originalTeam = new TeamBuilder()
                .WithId(teamId)
                .WithName("Nome Antigo")
                .Build();

            dbContext.Teams.Add(originalTeam);
            await dbContext.SaveChangesAsync();

            // Limpar o tracker para simular novo pedido
            dbContext.ChangeTracker.Clear();

            var updatedTeam = new TeamBuilder()
                .WithId(teamId)
                .WithName("Nome Atualizado")
                .Build();

            await repository.SaveAsync(updatedTeam);

            var result = await dbContext.Teams.AsNoTracking().FirstOrDefaultAsync(t => t.Id == teamId);
            result.Should().NotBeNull();
            result.Name.Should().Be("Nome Atualizado");
            dbContext.Teams.Should().HaveCount(1); // Verifica que foi Update e não Insert
        }

        // ==========================================
        // TESTES DO MÉTODO: GetByIdAsync
        // ==========================================

        [Fact]
        public async Task GetByIdAsync_ShouldReturnTeam_WhenExists()
        {
            var dbContext = GetDbContext();
            var repository = new TeamRepository(dbContext);

            var teamId = Guid.NewGuid();
            var team = new TeamBuilder().WithId(teamId).WithName("Data Science").Build();

            dbContext.Teams.Add(team);
            await dbContext.SaveChangesAsync();

            var result = await repository.GetByIdAsync(teamId);

            result.Should().NotBeNull();
            result.Id.Should().Be(teamId);
            result.Name.Should().Be("Data Science");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenNotExists()
        {
            var dbContext = GetDbContext();
            var repository = new TeamRepository(dbContext);

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
            var repository = new TeamRepository(dbContext);

            // Inserir 3 equipas desordenadas alfabeticamente
            var team1 = new TeamBuilder().WithId(Guid.NewGuid()).WithName("Zeta").Build();
            var team2 = new TeamBuilder().WithId(Guid.NewGuid()).WithName("Alpha").Build();
            var team3 = new TeamBuilder().WithId(Guid.NewGuid()).WithName("Beta").Build();

            dbContext.Teams.AddRange(team1, team2, team3);
            await dbContext.SaveChangesAsync();

            // Pedir a página 1, tamanho 2.
            // A Query faz OrderBy(Name), as equipas esperadas são "Alpha" e "Beta".
            var result = await repository.GetPagedAsync(page: 1, size: 2);

            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result[0].Name.Should().Be("Alpha");
            result[1].Name.Should().Be("Beta");
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