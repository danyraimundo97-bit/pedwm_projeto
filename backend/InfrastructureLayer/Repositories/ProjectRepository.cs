using DomainLayer.Domain.Projects;
using ApplicationLayer.Repositories;
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
            // TODO: Verificar se o projeto já existe e atualizar em vez de criar um novo, para evitar duplicados
            //TODO: Adicionar tratamento de erros (try-catch) para lidar com possíveis falhas na base de dados
            //TODO: Implementar logging mais detalhado (ex: sucesso, falha)
            //TODO: Verificar se o projeto é válido antes de tentar salvar (ex: campos obrigatórios)
            LoggerService.Instance.LogInfo($"[DATABASE] A guardar o projeto {project.Id} na BD...");

            _context.Projects.Add(project); // Adiciona o projeto ao DbSet
            await _context.SaveChangesAsync(); // Salva as alterações na base de dados d Salva as alterações na base de dados

            //TODO: Verificar o resultado do SaveChangesAsync para confirmar que a operação foi bem-sucedida
            //TODO: Logar o resultado da operação

        }

        public async Task<IReadOnlyList<ProjectBase>> GetAllAsync()
        {
            LoggerService.Instance.LogInfo("[DATABASE] A ler todos os projetos...");
            return await _context.Projects.ToListAsync();
            //TODO: Adicionar tratamento de erros (try-catch) para lidar com possíveis falhas na base de dados
            //TODO: Implementar logging mais detalhado (ex: número de projetos lidos, falha)
            //TODO: Implementar paginação para evitar ler uma quantidade excessiva de dados de uma só vez
        }
    }
}
