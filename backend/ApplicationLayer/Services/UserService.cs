using System;
using System.Threading.Tasks;
using ApplicationLayer.Commands;
using ApplicationLayer.Repositories;
using DomainLayer.Domain.Builders;
using DomainLayer.Domain.Users;

namespace ApplicationLayer.Services
{
    // Serviço responsável por aplicar as regras de negócio antes de criar um User.
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IAppLogger _logger;

        public UserService(IUserRepository repository, IAppLogger logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<User> CreateUserAsync(CreateUserCommand command)
        {
            // --- VALIDAÇÕES DE NEGÓCIO ---

            // O Nome e o Email são obrigatórios
            if (string.IsNullOrWhiteSpace(command.Name) || string.IsNullOrWhiteSpace(command.Email))
            {
                throw new ArgumentException("O Nome e o Email são obrigatórios.");
            }

            // --- CRIAÇÃO DA ENTIDADE ---
            // Mapeamos os dados do comando para a nossa entidade User
            var user = new UserBuilder()
                .WithId(Guid.NewGuid())
                .WithName(command.Name)
                .WithRole(command.Role)
                .WithEmail(command.Email)
                .Build();

            // --- PERSISTÊNCIA ---
            // Guarda o user na Base de Dados através do Repositório
            await _repository.SaveAsync(user);
            _logger.LogInfo($"[SERVICE] Utilizador '{user.Name}' guardado com sucesso!");

            // Devolve o user criado para o Handler
            return user;
        }
    }
}