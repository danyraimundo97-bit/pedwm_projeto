using System;
using System.Threading.Tasks;
using ApplicationLayer.Commands;
using ApplicationLayer.Factories;
using DomainLayer.Domain.Repositories;
using DomainLayer.Domain.Tasks;

namespace ApplicationLayer.Services
{
    // Serviço responsável por aplicar as regras de negócio antes de criar uma Tarefa.
    public class TaskService : ITaskService
    {
        private readonly ProjectTaskFactory _factory;
        private readonly ITaskRepository _repository;
        private readonly IAppLogger _logger;

        // Construtor: Injeta a Factory e o Repositório de Tarefas
        public TaskService(ProjectTaskFactory factory, ITaskRepository repository, IAppLogger logger)
        {
            _factory = factory;
            _repository = repository;
            _logger = logger;
        }

        public async Task<TaskBase> CreateTaskAsync(CreateTaskCommand command)
        {
            // --- VALIDAÇÕES DE NEGÓCIO ---

            // Uma tarefa não pode existir sem título
            if (string.IsNullOrWhiteSpace(command.Title))
            {
                _logger.Log("[SERVICE] Erro de Validação: Título da tarefa em branco.");
                throw new ArgumentException("O título da tarefa é obrigatório.");
            }

            // Garantir que a tarefa está associada a um projeto válido
            if (command.ProjectId == Guid.Empty)
            {
                _logger.Log("[SERVICE] Erro de Validação: ID do Projeto inválido.");
                throw new ArgumentException("A tarefa tem de estar associada a um Projeto válido.");
            }

            // --- CRIAÇÃO DA ENTIDADE ---
            // Pede à Factory para construir a Feature ou o Bug
            var task = _factory.CreateFromCommand(command);

            // --- PERSISTÊNCIA ---
            // Guarda a tarefa na Base de Dados através do Repositório
            await _repository.SaveAsync(task);
            _logger.Log($"[SERVICE] Tarefa '{task.Title}' processada e guardada com sucesso!");

            // Devolve a tarefa criada para o Handler
            return task;
        }
    }
}