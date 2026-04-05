using ApplicationLayer.Commands;
using DomainLayer.Domain.Builders;
using DomainLayer.Domain.Tasks;

namespace ApplicationLayer.Factories
{
    /// <summary>Maps application commands to domain aggregates (use-case construction).</summary>
    public static class TaskFactory
    {

        public static TaskBase Create(CreateTaskCommand cmd)
        {
            return cmd.Type switch
            {
                TaskType.Bug => BugTask.Builder()
                    .WithTitle(cmd.Title)
                    .WithDescription(cmd.Description)
                    .InProject(cmd.ProjectId)
                    .ForEnvironment(cmd.Environment ?? "Production")
                    .AssignedTo(cmd.AssignedUserId)
                    .Build(),

                TaskType.Feature => FeatureTask.Builder()
                    .WithTitle(cmd.Title)
                    .WithDescription(cmd.Description)
                    .InProject(cmd.ProjectId)
                    .WithStoryPoints(cmd.StoryPoints ?? 0)
                    .AssignedTo(cmd.AssignedUserId)
                    .Build(),

                _ => throw new ArgumentException($"Tipo de tarefa '{cmd.Type}' desconhecido."),
            };
        }
    }
}
