using System;
using System.Threading.Tasks;
using ApplicationLayer.Commands;
using ApplicationLayer.Repositories;
using DomainLayer.Domain.Teams;

namespace ApplicationLayer.Services
{
    // Serviço responsável por aplicar as regras de negócio antes de criar uma Equipa.
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _repository;
        private readonly IAppLogger _logger;

        public TeamService(ITeamRepository repository, IAppLogger logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Team> CreateTeamAsync(CreateTeamCommand command)
        {
            // --- VALIDAÇÕES DE NEGÓCIO ---

            // O nome da equipa é obrigatório
            if (string.IsNullOrWhiteSpace(command.Name))
            {
                _logger.LogInfo($"[SERVICE] Erro de Validação: Nome da equipa em branco.");
                throw new ArgumentException("O nome da equipa é obrigatório.");
            }

            // --- CRIAÇÃO DA ENTIDADE ---
            // Criamos a entidade Team com os dados do comando
            var team = new Team { Name = command.Name };

            // --- PERSISTÊNCIA ---
            // Guarda a equipa na Base de Dados através do Repositório
            await _repository.SaveAsync(team);
            _logger.LogInfo($"[SERVICE] Equipa '{team.Name}' guardada com sucesso!");

            // Devolve a equipa criada para o Handler
            return team;
        }
    }
}