using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ApplicationLayer.Repositories;
using DomainLayer.Domain;
using DomainLayer.Domain.Projects;
using InfrastructureLayer.Data;
using InfrastructureLayer.Patterns.Singleton;

namespace InfrastructureLayer.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly AppDbContext _context;

        // Injeção da dependência do DbContext para aceder à base de dados
        public ProjectRepository(AppDbContext context)
        {
            _context = context;
        }

        // Salva um projeto na base de dados de forma assíncrona
        public async Task SaveAsync(ProjectBase project)
        {
            LoggerService.Instance.Log($"[DATABASE] A guardar o projeto {project.Id} na BD...");

            _context.Projects.Add(project); // Adiciona o projeto ao DbSet
            await _context.SaveChangesAsync(); // Salva as alterações na base de dados
        }

        // Obter todos os projetos da base de dados de forma assíncrona
        public async Task<IEnumerable<ProjectBase>> GetAllAsync()
        {
            LoggerService.Instance.Log("[DATABASE] A ler todos os projetos...");
            // O EF Core vai à tabela e devolve a lista completa!
            return await _context.Projects.ToListAsync();
        }
    }
}