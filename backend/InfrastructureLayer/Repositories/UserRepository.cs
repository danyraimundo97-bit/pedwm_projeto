using ApplicationLayer.Repositories;
using DomainLayer.Domain.Users;
using InfrastructureLayer.Data;
using InfrastructureLayer.Patterns.Singleton;
using Microsoft.EntityFrameworkCore;

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

        // Save
        public async Task SaveAsync(User user)
        {
            LoggerService.Instance.LogInfo($"[DATABASE] A processar o Utilizador '{user.Name}' (ID: {user.Id})...");

            // Verificar se a tarefa já existe ('AsNoTracking' para performance)
            bool exists = await _context.Users.AsNoTracking().AnyAsync(u => u.Id == user.Id);

            if (exists)
            {
                _context.Users.Update(user);
                LoggerService.Instance.LogInfo($"[DATABASE] A atualizar o Utilizador {user.Id}.");
            }
            else
            {
                await _context.Users.AddAsync(user);
                LoggerService.Instance.LogInfo($"[DATABASE] A inserir o Utilizador {user.Id}.");
            }

            // Verificar o resultado
            int rowsAffected = await _context.SaveChangesAsync();
            LoggerService.Instance.LogInfo($"[DATABASE] Operação concluída com {rowsAffected} linhas afetadas");
        }

        // GET ALL
        public async Task<IReadOnlyList<User>> GetAllAsync()
        {
            LoggerService.Instance.LogInfo("[DATABASE] A iniciar leitura de todos os utilizadores...");

            var users = await _context.Users.ToListAsync();

            LoggerService.Instance.LogInfo($"[DATABASE] Leitura de {users.Count} utilizadores concluída.");

            return users;
        }
    }
}