using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationLayer.Queries;
using ApplicationLayer.Models;
using Mapster;
using HotChocolate;

namespace PresentationLayer.GraphQL
{
    public class Query
    {
        public string BemVindo() => "API de Gestão de Projetos Online - Leituras Ativas!";

        // ==========================================
        // PROJETOS
        // ==========================================


        // Listar Projetos (paged)
        public async Task<IReadOnlyList<ProjectSender>> GetProjects([Service] ListProjectsQueryHandler handler)
        {
            return await handler.HandleAsync();
        }


        // Procurar Projeto por ID
        public async Task<ProjectSender> GetProjectById(Guid id, [Service] GetProjectByIdQueryHandler handler)
        {
            var project = await handler.HandleAsync(id);

            if (project == null)
                throw new GraphQLException($"O Projeto com o ID {id} não foi encontrado.");

            return project;
        }

        // Procurar Projetos de um Utilizador
        public async Task<IReadOnlyList<ProjectSender>> GetProjectsByUser(Guid userId, [Service] GetProjectsByUserQueryHandler handler)
        {
            return await handler.HandleAsync(userId);
        }

        // ==========================================
        // TAREFAS
        // ==========================================

        // Listar Tarefas (paged)
        public async Task<IReadOnlyList<TaskSender>> GetTasks([Service] ListTasksQueryHandler handler)
        {
            return await handler.HandleAsync();
        }


        // Procurar Tarefa por ID
        public async Task<TaskSender> GetTaskById(Guid id, [Service] GetTaskByIdQueryHandler handler)
        {
            var task = await handler.HandleAsync(id);

            if (task == null)
                throw new GraphQLException($"A Tarefa com o ID {id} não foi encontrada.");

            return task;
        }

        // Procurar Tarefas de um Projeto
        public async Task<IReadOnlyList<TaskSender>> GetTasksByProject(Guid projectId, [Service] GetTasksByProjectQueryHandler handler)
        {
            return await handler.HandleAsync(projectId);
        }

        // Procurar Tarefas de um Utilizador
        public async Task<IReadOnlyList<TaskSender>> GetTasksByUser(Guid userId, [Service] GetTasksByUserQueryHandler handler)
        {
            return await handler.HandleAsync(userId);
        }

        // ==========================================
        // EQUIPAS (TEAMS)
        // ==========================================

        // Listar Equipas (paged)
        public async Task<IReadOnlyList<TeamSender>> GetTeams([Service] ListTeamsQueryHandler handler)
        {
            return await handler.HandleAsync();
        }

        // Procurar Equipas de um Utilizador
        public async Task<TeamSender> GetTeamById(Guid id, [Service] GetTeamByIdQueryHandler handler)
        {
            var team = await handler.HandleAsync(id);

            if (team == null)
                throw new GraphQLException($"A Equipa com o ID {id} não foi encontrada.");

            return team;
        }

        // ==========================================
        // UTILIZADORES (USERS)
        // ==========================================

        // Listar Utilizadores
        public async Task<IReadOnlyList<UserResponse>> GetUsers([Service] ListUsersQueryHandler handler)
        {
            return await handler.HandleAsync();
        }

        // Procurar Utilizador por ID
        public async Task<UserResponse> GetUserById(Guid id, [Service] GetUserByIdQueryHandler handler)
        {
            var user = await handler.HandleAsync(id);

            if (user == null)
                throw new GraphQLException($"O Utilizador com o ID {id} não foi encontrado.");

            return user;
        }

        public async Task<IReadOnlyList<HourLogResponse>> GetHourLogs(
            DateTime from,
            DateTime to,
            [Service] ListHourLogsQueryHandler handler)
        {
            return await handler.HandleAsync(from, to);
        }
    }
}
