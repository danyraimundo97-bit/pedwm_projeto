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
            // TODO: Verificar se o projeto já existe e atualizar em vez de criar um novo, para evitar duplicados
            //TODO: Adicionar tratamento de erros (try-catch) para lidar com possíveis falhas na base de dados
            //TODO: Implementar logging mais detalhado (ex: sucesso, falha)
            //TODO: Verificar se o projeto é válido antes de tentar salvar (ex: campos obrigatórios)
            LoggerService.Instance.Log($"[DATABASE] A guardar o projeto {project.Id} na BD...");

            _context.Projects.Add(project); // Adiciona o projeto ao DbSet
            await _context.SaveChangesAsync(); // Salva as alterações na base de dados d Salva as alterações na base de dados

            //TODO: Verificar o resultado do SaveChangesAsync para confirmar que a operação foi bem-sucedida
            //TODO: Logar o resultado da operação

        }

        // Obter todos os projetos da base de dados de forma assíncrona
        public async Task<IEnumerable<ProjectBase>> GetAllAsync()
        {
            LoggerService.Instance.Log("[DATABASE] A ler todos os projetos...");
            // O EF Core vai à tabela e devolve a lista completa!
            return await _context.Projects.ToListAsync();
            //TODO: Adicionar tratamento de erros (try-catch) para lidar com possíveis falhas na base de dados
            //TODO: Implementar logging mais detalhado (ex: número de projetos lidos, falha)
            //TODO: Implementar paginação para evitar ler uma quantidade excessiva de dados de uma só vez
        }
    }
}