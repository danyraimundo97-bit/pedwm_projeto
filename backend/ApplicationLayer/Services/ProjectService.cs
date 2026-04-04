using System;
using System.Threading.Tasks;
using ApplicationLayer.Commands;
using ApplicationLayer.Factories;
using ApplicationLayer.Repositories;
using DomainLayer.Domain.Projects;
using ProjectEntity = DomainLayer.Domain.Projects.Project;

namespace ApplicationLayer.Services
{
    // Serviço responsável por aplicar as regras de negócio antes de criar um Projeto.
    // Retira esta responsabilidade do Handler para manter o código organizado e fácil de testar.
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _repository;
        private readonly IAppLogger _logger;

        // Construtor: Injeta a Factory e o Repositório de Projetos
        public ProjectService(IProjectRepository repository, IAppLogger logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<ProjectBase> CreateProjectAsync(CreateProjectCommand command)
        {
            // --- VALIDAÇÕES DE NEGÓCIO ---

            // Verifica se as datas fazem sentido cronológico
            if (command.EndDate < command.StartDate)
            {
                _logger.LogError("[SERVICE] Erro de Validação: Data de fim do projeto anterior à data de início do projeto.");
                throw new ArgumentException("A data de término não pode ser anterior à data de início.");
            }

            // Verifica se o projeto tem tempo alocado válido
            if (command.AllocatedHours <= 0)
            {
                _logger.LogError("[SERVICE] Erro de Validação: Horas alocadas ao projeto inválidas.");
                throw new ArgumentException("As horas alocadas devem ser maiores que zero.");
            }

            // --- CRIAÇÃO DA ENTIDADE ---
            // Se passou nas validações, pede à Factory para construir o objeto de Domínio
            var project = ProjectFactory.Create(command);

            // --- PERSISTÊNCIA ---
            // Guarda o projeto na Base de Dados através do Repositório
            await _repository.SaveAsync(project);
            _logger.LogInfo($"[SERVICE] Projeto '{project.Title}' criado e guardado com sucesso!");

            // Devolve o projeto criado para o Handler
            return project;
        }

        public async Task<ProjectBase> AddConsumedHoursToProjectAsync(AddHoursToProjectCommand command)
        {
            if (string.IsNullOrWhiteSpace(command.ProjectId))
            {
                throw new ArgumentException("O identificador do projeto é obrigatório.");
            }

            if (command.Hours <= 0)
            {
                throw new ArgumentException("As horas devem ser um valor positivo.");
            }

            if (!Guid.TryParse(command.ProjectId, out var projectId))
            {
                throw new ArgumentException("ID do projeto inválido.");
            }

            var project = await _repository.GetByIdAsync(projectId);
            if (project is not ProjectEntity standard)
            {
                throw new InvalidOperationException("Apenas projetos standard suportam registo de horas consumidas.");
            }

            standard.AddConsumedHours(command.Hours);
            await _repository.SaveAsync(standard);
            _logger.LogInfo($"[SERVICE] Registadas {command.Hours} horas no projeto '{standard.Title}'.");

            return standard;
        }

        public async Task<ProjectBase> ChangeProjectStatusAsync(ChangeProjectStatusCommand command)
        {
            if (string.IsNullOrWhiteSpace(command.ProjectId))
            {
                throw new ArgumentException("O identificador do projeto é obrigatório.");
            }

            if (!Guid.TryParse(command.ProjectId, out var projectId))
            {
                throw new ArgumentException("ID do projeto inválido.");
            }

            var project = await _repository.GetByIdAsync(projectId);
            if (project is not ProjectEntity standard)
            {
                throw new InvalidOperationException("Apenas projetos standard suportam alteração de estado.");
            }

            standard.ChangeStatus(command.Status);
            await _repository.SaveAsync(standard);
            _logger.LogInfo($"[SERVICE] Estado do projeto '{standard.Title}' alterado para {command.Status}.");

            return standard;
        }
    }
}