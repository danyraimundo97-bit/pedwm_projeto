using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationLayer.Repositories;
using HotChocolate;
using PresentationLayer.DTOs;
using Mapster;

namespace PresentationLayer.GraphQL
{
    public class Query
    {
        public string BemVindo() => "API de Gestão de Projetos Online - Leituras Ativas!";

        // ==========================================
        // PROJETOS
        // ==========================================

        public async Task<List<ProjectResponse_DTO>> GetProjects(
            [Service] IProjectRepository repository,
            int page = 1, int size = 10)
        {
            var projects = await repository.GetPagedAsync(page, size);
            // O Mapster converte a IReadOnlyList<ProjectBase> para List<ProjectResponse_DTO> automaticamente!
            return projects.Adapt<List<ProjectResponse_DTO>>();
        }

        public async Task<ProjectResponse_DTO> GetProjectById(Guid id, [Service] IProjectRepository repository)
        {
            var project = await repository.GetByIdAsync(id);

            if (project == null)
                throw new GraphQLException($"O Projeto com o ID {id} não foi encontrado.");

            return project.Adapt<ProjectResponse_DTO>();
        }

        // Buscar Projetos de um Utilizador específico
        public async Task<List<ProjectResponse_DTO>> GetProjectsByUser(
            Guid userId, [Service] IProjectRepository repository)
        {
            var projects = await repository.GetByUserAsync(userId);
            return projects.Adapt<List<ProjectResponse_DTO>>();
        }

        // ==========================================
        // TAREFAS
        // ==========================================

        public async Task<List<TaskResponse_DTO>> GetTasks(
            [Service] ITaskRepository repository,
            int page = 1, int size = 10)
        {
            var tasks = await repository.GetPagedAsync(page, size);
            return tasks.Adapt<List<TaskResponse_DTO>>();
        }

        public async Task<TaskResponse_DTO> GetTaskById(
            Guid id, [Service] ITaskRepository repository)
        {
            var task = await repository.GetByIdAsync(id);

            if (task == null)
                throw new GraphQLException($"A Tarefa com o ID {id} não foi encontrada.");

            return task.Adapt<TaskResponse_DTO>();
        }

        // Buscar Tarefas de um Projeto específico
        public async Task<List<TaskResponse_DTO>> GetTasksByProject(Guid projectId, [Service] ITaskRepository repository)
        {
            var tasks = await repository.GetByProjectAsync(projectId);
            return tasks.Adapt<List<TaskResponse_DTO>>();
        }

        // Buscar Tarefas de um Utilizador específico
        public async Task<List<TaskResponse_DTO>> GetTasksByUser(Guid userId, [Service] ITaskRepository repository)
        {
            var tasks = await repository.GetByUserAsync(userId);
            return tasks.Adapt<List<TaskResponse_DTO>>();
        }

        // ==========================================
        // EQUIPAS (TEAMS)
        // ==========================================

        public async Task<List<TeamResponse_DTO>> GetTeams(
            [Service] ITeamRepository repository,
            int page = 1, int size = 10)
        {
            var teams = await repository.GetPagedAsync(page, size);
            return teams.Adapt<List<TeamResponse_DTO>>();
        }

        public async Task<TeamResponse_DTO> GetTeamById(
            Guid id, [Service] ITeamRepository repository)
        {
            var team = await repository.GetByIdAsync(id);

            if (team == null)
                throw new GraphQLException($"A Equipa com o ID {id} não foi encontrada.");

            return team.Adapt<TeamResponse_DTO>();
        }

        // ==========================================
        // UTILIZADORES (USERS)
        // ==========================================

        public async Task<List<UserResponse_DTO>> GetUsers(
            [Service] IUserRepository repository,
            int page = 1, int size = 10)
        {
            var users = await repository.GetPagedAsync(page, size);
            return users.Adapt<List<UserResponse_DTO>>();
        }

        public async Task<UserResponse_DTO> GetUserById(
            Guid id, [Service] IUserRepository repository)
        {
            var user = await repository.GetByIdAsync(id);

            if (user == null)
                throw new GraphQLException($"O Utilizador com o ID {id} não foi encontrado.");

            return user.Adapt<UserResponse_DTO>();
        }
    }
}