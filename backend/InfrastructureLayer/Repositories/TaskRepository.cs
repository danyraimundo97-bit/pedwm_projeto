using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DomainLayer.Domain.Tasks;
using ApplicationLayer.Repositories;
using InfrastructureLayer.Data;
using InfrastructureLayer.Patterns.Singleton;

namespace InfrastructureLayer.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly AppDbContext _context;

        // Injeção da dependência do DbContext para aceder à base de dados
        public TaskRepository(AppDbContext context)
        {
            _context = context;
        }

        // SAVE
        public async Task SaveAsync(TaskBase task)
        {
            LoggerService.Instance.LogInfo($"[DATABASE] A processar a Tarefa '{task.Title}' (ID: {task.Id})...");

            // Verificar se a tarefa já existe ('AsNoTracking' para performance)
            bool exists = await _context.Tasks.AsNoTracking().AnyAsync(t => t.Id == task.Id);

            if (exists)
            {
                _context.Tasks.Update(task);
                LoggerService.Instance.LogInfo($"[DATABASE] A atualizar a Tarefa {task.Id}.");
            }
            else
            {
                await _context.Tasks.AddAsync(task);
                LoggerService.Instance.LogInfo($"[DATABASE] A inserir a Tarefa {task.Id}.");
            }

            // Verificar o resultado
            int rowsAffected = await _context.SaveChangesAsync();
            LoggerService.Instance.LogInfo($"[DATABASE] Operação concluída com {rowsAffected} linhas afetadas");
        }

        // GET PAGED
        public async Task<IReadOnlyList<TaskBase>> GetPagedAsync(int page, int size)
        {
            LoggerService.Instance.LogInfo($"[DATABASE] A ler tarefas (Página {page}, Tamanho {size})...");
            return await _context.Tasks
                .AsNoTracking()
                .OrderBy(t => t.Id) // Ordena por ID
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();
        }

        // GET TASK BY ID
        public async Task<TaskBase?> GetByIdAsync(Guid id)
        {
            return await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<TaskBase?> GetTaskAsync(string taskId, string projectId)
        {
            if (!Guid.TryParse(taskId, out var tid) || !Guid.TryParse(projectId, out var pid))
                return null;

            return await _context.Tasks.FirstOrDefaultAsync(t => t.Id == tid && t.ProjectId == pid);
        }

        // GET TASKS BY PROJECT
        public async Task<IReadOnlyList<TaskBase>> GetByProjectAsync(Guid projectId)
        {
            LoggerService.Instance.LogInfo($"[DATABASE] A ler tarefas do projeto {projectId}...");
            return await _context.Tasks
                .AsNoTracking()
                .Where(t => t.ProjectId == projectId)
                .ToListAsync();
        }

        // GET TASKS BY USER
        public async Task<IReadOnlyList<TaskBase>> GetByUserAsync(Guid userId)
        {
            LoggerService.Instance.LogInfo($"[DATABASE] A ler tarefas atribuídas ao utilizador {userId}...");
            return await _context.Tasks
                .AsNoTracking()
                .Where(t => t.AssignedUserId == userId)
                .ToListAsync();
        }
    }
}