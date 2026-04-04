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
        private readonly ITeamRepository _teamRepository;
        private readonly IAppLogger _logger;

        public UserService(IUserRepository repository, ITeamRepository teamRepository, IAppLogger logger)
        {
            _repository = repository;
            _teamRepository = teamRepository;
            _logger = logger;
        }

        public async Task<User> CreateUserAsync(CreateUserCommand command)
        {
            // --- VALIDAÇÕES DE NEGÓCIO ---

            // O Nome e o Email são obrigatórios
            if (string.IsNullOrWhiteSpace(command.Name) || string.IsNullOrWhiteSpace(command.Email))
            {
                _logger.LogWarning($"[SERVICE] Erro de Validação: Nome ou Email do utilizador em branco.");
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

        public async Task<User> AssignUserToTeamAsync(AssignUserToTeamCommand command)
        {
            if (string.IsNullOrWhiteSpace(command.UserId) || string.IsNullOrWhiteSpace(command.TeamId))
            {
                _logger.LogWarning("[SERVICE] Utilizador ou equipa em falta.");
                throw new ArgumentException("O identificador do utilizador e da equipa são obrigatórios.");
            }

            if (!Guid.TryParse(command.UserId, out var userId) || !Guid.TryParse(command.TeamId, out var teamId))
            {
                throw new ArgumentException("IDs inválidos.");
            }

            var user = await _repository.GetByIdAsync(userId);
            if (user is null)
            {
                throw new InvalidOperationException("Utilizador não encontrado.");
            }

            var team = await _teamRepository.GetByIdAsync(teamId);
            if (team is null)
            {
                throw new InvalidOperationException("Equipa não encontrada.");
            }

            user.TeamId = teamId;
            await _repository.SaveAsync(user);
            _logger.LogInfo($"[SERVICE] Utilizador '{user.Name}' associado à equipa '{team.Name}'.");

            return user;
        }
    }
}