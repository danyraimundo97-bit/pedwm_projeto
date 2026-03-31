using System;
using System.Collections.Generic;
using DomainLayer.Domain.Builders;
using DomainLayer.Domain.Tasks;
using ApplicationLayer.Commands;

namespace ApplicationLayer.Factories
{
    public class ProjectTaskFactory
    {
        private readonly Dictionary<TaskType, Func<CreateTaskCommand, TaskBase>> _buildersMap;

        public ProjectTaskFactory()
        {
            // Inicializa o dicionário de builders para cada tipo de tarefa
            _buildersMap = new Dictionary<TaskType, Func<CreateTaskCommand, TaskBase>>()
            {
                { TaskType.Bug, cmd => new BugTaskBuilder()
                    .WithTitle(cmd.Title)
                    .WithDescription(cmd.Description)
                    .InProject(cmd.ProjectId)
                    .ForEnvironment(cmd.Environment ?? "Production")
                    .Build() },

                { TaskType.Feature, cmd => new FeatureTaskBuilder()
                    .WithTitle(cmd.Title)
                    .WithDescription(cmd.Description)
                    .InProject(cmd.ProjectId)
                    .WithStoryPoints(cmd.StoryPoints ?? 0)
                    .Build() }
            };
        }

        // Método para criar uma tarefa utilizando o dicionário de builders
        public TaskBase CreateFromCommand(CreateTaskCommand cmd)
        {
            if (_buildersMap.TryGetValue(cmd.Type, out var builderFunc))
            {
                return builderFunc(cmd);
            }

            throw new ArgumentException($"Tipo de tarefa '{cmd.Type}' desconhecido.");
        }
    }
}