using DomainLayer.Domain.Projects;
using DomainLayer.Ports;
using InfrastructureLayer.Data;
using InfrastructureLayer.Patterns.Singleton;
using Microsoft.EntityFrameworkCore;

namespace InfrastructureLayer.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly AppDbContext _context;

        public ProjectRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task SaveAsync(ProjectBase project)
        {
            LoggerService.Instance.Log($"[DATABASE] A guardar o projeto {project.Id} na BD...");

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<ProjectBase>> GetAllAsync()
        {
            LoggerService.Instance.Log("[DATABASE] A ler todos os projetos...");
            return await _context.Projects.ToListAsync();
        }
    }
}
