using ApplicationLayer.Handlers;
using ApplicationLayer.Repositories;
using HotChocolate;
using PresentationLayer.DTOs;
using PresentationLayer.GraphQL.Mapping;
using PresentationLayer.Interfaces;

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
    }
}
