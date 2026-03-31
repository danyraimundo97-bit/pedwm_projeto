using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DomainLayer.Domain.Teams;
using InfrastructureLayer.Data;
using InfrastructureLayer.Patterns.Singleton;
using ApplicationLayer.Repositories;

namespace InfrastructureLayer.Repositories
{
    public class TeamRepository : ITeamRepository
    {
        private readonly AppDbContext _context;

        // Injeção da dependência do DbContext para aceder à base de dados

        public TeamRepository(AppDbContext context)
        {
            _context = context;
        }

        // Salva uma equipa na base de dados de forma assíncrona

        public async Task SaveAsync(Team team)
        {
            LoggerService.Instance.LogInfo($"[DATABASE] A guardar equipa {team.Name} na BD...");
            
            _context.Teams.Add(team);   // Adiciona a team ao DbSet
            await _context.SaveChangesAsync();  // Salva as alterações na base de dados
        }

        // Obter todas as equipas da base de dados de forma assíncrona
        public async Task<IEnumerable<Team>> GetAllAsync()
        {
            LoggerService.Instance.LogInfo("[DATABASE] A ler todas as equipas...");

            // O .Include(t => t.Members) diz ao EF Core: 
            // "Quando fores buscar a equipa, vai também à tabela dos Users e junta as pessoas à lista!"
            return await _context.Teams
                .Include(t => t.Members)
                .ToListAsync();
        }
    }
}