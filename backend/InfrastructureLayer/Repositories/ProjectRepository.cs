using Microsoft.EntityFrameworkCore;
using DomainLayer.Domain.Projects;
using ApplicationLayer.Repositories;
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

        // SAVE
        public async Task SaveAsync(ProjectBase project)
        {
            LoggerService.Instance.LogInfo($"[DATABASE] A processar o Projeto '{project.Title}' (ID: {project.Id})...");

            // Verificar se o projeto já existe ('AsNoTracking' para performance)
            bool exists = await _context.Projects.AsNoTracking().AnyAsync(p => p.Id == project.Id);

            if (exists)
            {
                _context.Projects.Update(project);
                LoggerService.Instance.LogInfo($"[DATABASE] A atualizar o Projeto {project.Id}.");
            }
            else
            {
                await _context.Projects.AddAsync(project);
                LoggerService.Instance.LogInfo($"[DATABASE] A inserir o Projeto {project.Id}.");
            }

            // Verificar o resultado
            int rowsAffected = await _context.SaveChangesAsync();
            LoggerService.Instance.LogInfo($"[DATABASE] Operação concluída com {rowsAffected} linhas afetadas");
        }

        public async Task<ProjectBase?> GetByIdAsync(Guid id)
        {
            return await _context.Projects.FirstOrDefaultAsync(p => p.Id == id);
        }

        // GET ALL
        public async Task<IReadOnlyList<ProjectBase>> GetAllAsync()
        {
            LoggerService.Instance.LogInfo("[DATABASE] A iniciar a leitura de todos os projetos...");

            var projects = await _context.Projects.ToListAsync();

            LoggerService.Instance.LogInfo($"[DATABASE] Leitura de {projects.Count} projetos concluída.");

            return projects;
        }
    }
}