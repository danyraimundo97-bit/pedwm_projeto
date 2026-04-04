using ApplicationLayer.Commands;
using ApplicationLayer.Factories;
using ApplicationLayer.Repositories;
using DomainLayer.Domain.Tasks;
using DomainLayer.Domain.Users;
using TaskFactory = ApplicationLayer.Factories.TaskFactory;

namespace ApplicationLayer.Services
{
    // Serviço responsável por aplicar as regras de negócio antes de criar uma Tarefa.
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _repository;
        private readonly IUserRepository _userRepository;
        private readonly IAppLogger _logger;

        public TaskService(ITaskRepository repository, IUserRepository userRepository, IAppLogger logger)
        {
            _repository = repository;
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<TaskBase> CreateTaskAsync(CreateTaskCommand command)
        {
            // --- VALIDAÇÕES DE NEGÓCIO ---

            // Uma tarefa não pode existir sem título
            if (string.IsNullOrWhiteSpace(command.Title))
            {
                _logger.LogWarning("[SERVICE] Erro de Validação: Título da tarefa em branco.");
                throw new ArgumentException("O título da tarefa é obrigatório.");
            }

            // Garantir que a tarefa está associada a um projeto válido
            if (command.ProjectId == Guid.Empty)
            {
                _logger.LogWarning("[SERVICE] Erro de Validação: ID do Projeto inválido.");
                throw new ArgumentException("A tarefa tem de estar associada a um Projeto válido.");
            }

            // --- CRIAÇÃO DA ENTIDADE ---
            // Pede à Factory para construir a Feature ou o Bug
            var task = TaskFactory.Create(command);

            // --- PERSISTÊNCIA ---
            // Guarda a tarefa na Base de Dados através do Repositório
            await _repository.SaveAsync(task);
            _logger.LogInfo($"[SERVICE] Tarefa '{task.Title}' processada e guardada com sucesso!");

            // Devolve a tarefa criada para o Handler
            return task;
        }

        public async Task<User> AssignUserToTaskAsync(string assigneeUserId, string taskId, string projectId)
        {
            if (string.IsNullOrWhiteSpace(assigneeUserId))
            {
                _logger.LogWarning("[SERVICE] Erro de Validação: Utilizador está em branco ou não existe");
                throw new ArgumentException("O Utilizador é obrigatório.");
            }

            if (string.IsNullOrWhiteSpace(taskId))
            {
                _logger.LogWarning("[SERVICE] Erro de Validação: Tarefa está em branco ou não existe");
                throw new ArgumentException("O Tarefa é obrigatório.");
            }

            if (string.IsNullOrWhiteSpace(projectId))
            {
                _logger.LogWarning("[SERVICE] Erro de Validação: Projeto está em branco ou não existe");
                throw new ArgumentException("O Projeto é obrigatório.");
            }

            if (!Guid.TryParse(assigneeUserId, out var assigneeGuid))
            {
                throw new ArgumentException("ID do utilizador inválido.");
            }

            var task = await _repository.GetTaskAsync(taskId, projectId);
            if (task is null)
            {
                _logger.LogWarning("[SERVICE] Tarefa não encontrada.");
                throw new InvalidOperationException("Tarefa não encontrada.");
            }

            var updated = TaskFactory.ChangeAssignee(task, assigneeUserId);
            await _repository.SaveAsync(updated);

            var user = await _userRepository.GetByIdAsync(assigneeGuid);
            if (user is null)
            {
                _logger.LogWarning("[SERVICE] Utilizador atribuído não encontrado na base de dados.");
                throw new InvalidOperationException("Utilizador não encontrado.");
            }

            return user;
        }
    }
}