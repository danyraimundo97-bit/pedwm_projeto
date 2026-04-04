using ApplicationLayer.Commands;
using DomainLayer.Domain.Builders;
using DomainLayer.Domain.Tasks;

namespace ApplicationLayer.Factories
{
    /// <summary>Maps application commands to domain aggregates (use-case construction).</summary>
    public static class TaskFactory
    {
        /// <summary>Reassigns a user by rebuilding the task with <see cref="BugTaskBuilder.From"/> / <see cref="FeatureTaskBuilder.From"/>.</summary>
        public static TaskBase ChangeAssignee(TaskBase task, string assigneeUserId)
        {
            if (!Guid.TryParse(assigneeUserId, out var userId))
                throw new ArgumentException("Invalid assignee user id.", nameof(assigneeUserId));

            task.ChangeAssignee(userId);

            return task;
        }

        public static TaskBase Create(CreateTaskCommand cmd)
        {
            return cmd.Type switch
            {
                TaskType.Bug => BugTask.Builder()
                    .WithTitle(cmd.Title)
                    .WithDescription(cmd.Description)
                    .InProject(cmd.ProjectId)
                    .ForEnvironment(cmd.Environment ?? "Production")
                    .Build(),

                TaskType.Feature => FeatureTask.Builder()
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
