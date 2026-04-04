using ApplicationLayer.Handlers;
using ApplicationLayer.Services;
using HotChocolate;
using PresentationLayer.DTOs;
using PresentationLayer.GraphQL.Mapping;

namespace PresentationLayer.GraphQL
{
    public class Mutation
    {

        private readonly IAppLogger _logger;

        public Mutation(IAppLogger logger)
        {
            _logger = logger;
        }

        public async Task<string> CreateProject(CreateProject_DTO input,[Service] CreateProjectHandler handler)
        {
            try
            {
                var command = input.ToCommand();
                var project = await handler.HandleAsync(command);
                return $"Projeto '{project.Title}' criado com o ID: {project.Id}";
            }
            catch (ArgumentException ex)
            {
                throw new GraphQLException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"[GraphQL Mutation] Falha ao tentar criar o Projeto com o título '{input.Title}'.", ex);
                throw new GraphQLException("Ocorreu um erro interno inesperado ao tentar criar o projeto.");
            }
        }

        public async Task<string> CreateTask(CreateTask_DTO input,[Service] CreateTaskHandler handler)
        {
            try
            {
                var command = input.ToCommand();
                var task = await handler.HandleAsync(command);
                return $"Tarefa '{task.Title}' criada com o ID: {task.Id}";
            }
            catch (ArgumentException ex)
            {
                throw new GraphQLException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"[GraphQL Mutation] Falha ao tentar criar a Tarefa com o título '{input.Title}'.", ex);
                throw new GraphQLException("Ocorreu um erro interno inesperado ao tentar criar a tarefa.");
            }
        }

        public async Task<string> CreateUser(CreateUser_DTO input, [Service] CreateUserHandler handler)
        {
            try
            {
                var command = input.ToCommand();
                var user = await handler.HandleAsync(command);
                return $"Utilizador '{user.Name}' criado com o email: {user.Email}";
            }
            catch (ArgumentException ex)
            {
                throw new GraphQLException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"[GraphQL Mutation] Falha ao tentar criar o Utilizador com o nome '{input.Name}' e com o email '{input.Email}'.", ex);
                throw new GraphQLException("Ocorreu um erro interno inesperado ao tentar criar o utilizador.");
            }
        }

        public async Task<string> CreateTeam(CreateTeam_DTO input, [Service] CreateTeamHandler handler)
        {
            try
            {
                var command = input.ToCommand();
                var team = await handler.HandleAsync(command);
                return $"Equipa '{team.Name}' criada com sucesso";
            }
            catch (ArgumentException ex)
            {
                throw new GraphQLException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"[GraphQL Mutation] Falha ao tentar criar a Equipa com o nome '{input.Name}'.", ex);
                throw new GraphQLException("Ocorreu um erro interno inesperado ao tentar criar a equipa.");
            }
        }

        public async Task<string> AssignTaskToUser(AssignUserToTask_DTO input, [Service] AssignUserToTaskHandler handler)
        {
            try
            {
                var command = input.ToCommand();
                await handler.HandleAsync(command);
                return "Tarefa atribuída ao utilizador com sucesso.";
            }
            catch (ArgumentException ex)
            {
                throw new GraphQLException(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                throw new GraphQLException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("[GraphQL Mutation] Falha ao atribuir tarefa ao utilizador.", ex);
                throw new GraphQLException("Ocorreu um erro interno inesperado ao atribuir a tarefa.");
            }
        }

        public async Task<string> AssignUserToTeam(AssignUserToTeam_DTO input, [Service] IUserService userService)
        {
            try
            {
                var command = input.ToCommand();
                var user = await userService.AssignUserToTeamAsync(command);
                return $"Utilizador '{user.Name}' associado à equipa com sucesso.";
            }
            catch (ArgumentException ex)
            {
                throw new GraphQLException(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                throw new GraphQLException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("[GraphQL Mutation] Falha ao associar utilizador à equipa.", ex);
                throw new GraphQLException("Ocorreu um erro interno inesperado ao associar o utilizador à equipa.");
            }
        }

        public async Task<string> AddHoursToProject(AddHoursToProject_DTO input, [Service] IProjectService projectService)
        {
            try
            {
                var command = input.ToCommand();
                var project = await projectService.AddConsumedHoursToProjectAsync(command);
                return $"Registadas {command.Hours} horas no projeto '{project.Title}'.";
            }
            catch (ArgumentException ex)
            {
                throw new GraphQLException(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                throw new GraphQLException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("[GraphQL Mutation] Falha ao registar horas no projeto.", ex);
                throw new GraphQLException("Ocorreu um erro interno inesperado ao registar horas.");
            }
        }

        public async Task<string> ChangeProjectStatus(ChangeProjectStatus_DTO input, [Service] IProjectService projectService)
        {
            try
            {
                var command = input.ToCommand();
                var project = await projectService.ChangeProjectStatusAsync(command);
                return $"Estado do projeto '{project.Title}' atualizado para {command.Status}.";
            }
            catch (ArgumentException ex)
            {
                throw new GraphQLException(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                throw new GraphQLException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("[GraphQL Mutation] Falha ao alterar estado do projeto.", ex);
                throw new GraphQLException("Ocorreu um erro interno inesperado ao alterar o estado.");
            }
        }
    }
}
