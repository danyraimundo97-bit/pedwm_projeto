using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        // GET BY ID
        public async Task<ProjectBase?> GetByIdAsync(Guid id)
        {
            return await _context.Projects.FirstOrDefaultAsync(p => p.Id == id);
        }

        // GET PAGED
        public async Task<IReadOnlyList<ProjectBase>> GetPagedAsync(int page, int size)
        {
            LoggerService.Instance.LogInfo($"[DATABASE] A ler projetos (Página {page}, Tamanho {size})...");
            
            return await _context.Projects
                .AsNoTracking()
                .OrderBy(p => p.StartDate) // Obrigatório para paginação funcionar bem
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();
        }

        // GET PROJECTS BY USER
        public async Task<IReadOnlyList<ProjectBase>> GetByUserAsync(Guid userId)
        {
            LoggerService.Instance.LogInfo($"[DATABASE] A procurar projetos onde o utilizador {userId} tem tarefas...");

            // Vai ás tasks e procura os IDs dos projetos do user
            var projectIds = await _context.Tasks
                .Where(t => t.AssignedUserId == userId)
                .Select(t => t.ProjectId)
                .Distinct() // Remove IDs duplicados
                .ToListAsync();

            if (!projectIds.Any())
            {
                return new List<ProjectBase>(); // Retorna vazio se não tiver tarefas
            }

            // Procura os projetos que correspondam a esses IDs
            return await _context.Projects
                .AsNoTracking()
                .Where(p => projectIds.Contains(p.Id))
                .ToListAsync();
        }
    }
}