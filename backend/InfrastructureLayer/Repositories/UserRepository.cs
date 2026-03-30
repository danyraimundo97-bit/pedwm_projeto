using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DomainLayer.Domain.Users;
using InfrastructureLayer.Data;
using InfrastructureLayer.Patterns.Singleton;
using ApplicationLayer.Repositories;

namespace InfrastructureLayer.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        // Injeção da dependência do DbContext para aceder à base de dados
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        // Salva um user na base de dados de forma assíncrona
        public async Task SaveAsync(User user)
        {
            LoggerService.Instance.Log($"[DATABASE] A guardar utilizador {user.Name} na BD...");
            
            _context.Users.Add(user);   // Adiciona o user ao DbSet
            await _context.SaveChangesAsync();  // Salva as alterações na base de dados
        }

        // Obter todos os users da base de dados de forma assíncrona
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            LoggerService.Instance.Log("[DATABASE] A ler todos os utilizadores...");
            return await _context.Users.ToListAsync();
        }
    }
}