using Microsoft.EntityFrameworkCore;
using DomainLayer.Domain.Teams;
using ApplicationLayer.Repositories;
using InfrastructureLayer.Data;
using InfrastructureLayer.Patterns.Singleton;

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

        // SAVE
        public async Task SaveAsync(Team team)
        {
            LoggerService.Instance.LogInfo($"[DATABASE] A processar a equipa '{team.Name}' (ID: {team.Id})...");

            // Verificar se a tarefa já existe ('AsNoTracking' para performance)
            bool exists = await _context.Teams.AsNoTracking().AnyAsync(t => t.Id == team.Id);

            if (exists)
            {
                _context.Teams.Update(team);
                LoggerService.Instance.LogInfo($"[DATABASE] A atualizar a Equipa {team.Id}.");
            }
            else
            {
                await _context.Teams.AddAsync(team);
                LoggerService.Instance.LogInfo($"[DATABASE] A inserir a Equipa {team.Id}.");
            }

            // Verificar o resultado
            int rowsAffected = await _context.SaveChangesAsync();
            LoggerService.Instance.LogInfo($"[DATABASE] Operação concluída com {rowsAffected} linhas afetadas");
        }

        // GET ALL
        public async Task<IReadOnlyList<Team>> GetAllAsync()
        {
            LoggerService.Instance.LogInfo("[DATABASE] A iniciar a leitura de todas as equipas...");

            // Include para carregar as relações dos team menbers
            var teams = await _context.Teams.Include(t => t.Members).ToListAsync();

            LoggerService.Instance.LogInfo($"[DATABASE] Leitura de {teams.Count} equipas concluída.");
            
            return teams;
        }
    }
}