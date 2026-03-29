using ApplicationLayer.Commands;
using ApplicationLayer.Models;
using DomainLayer.Domain.Tasks;
using AppTaskType = ApplicationLayer.Models.TaskType;

namespace ApplicationLayer.Factories
{
    /// <summary>Maps application commands to domain aggregates (use-case construction).</summary>
    public static class TaskFromCommandFactory
    {
        public static TaskBase Create(CreateTaskCommand cmd)
        {
            return cmd.Type switch
            {
                AppTaskType.Bug => BugTask.Builder()
                    .WithTitle(cmd.Title)
                    .WithDescription(cmd.Description)
                    .InProject(cmd.ProjectId)
                    .ForEnvironment(cmd.Environment ?? "Production")
                    .Build(),

                AppTaskType.Feature => FeatureTask.Builder()
                    .WithTitle(cmd.Title)
                    .WithDescription(cmd.Description)
                    .InProject(cmd.ProjectId)
                    .WithStoryPoints(cmd.StoryPoints ?? 0)
                    .Build(),

                _ => throw new ArgumentException($"Tipo de tarefa '{cmd.Type}' desconhecido."),
            };
        }
    }
}
