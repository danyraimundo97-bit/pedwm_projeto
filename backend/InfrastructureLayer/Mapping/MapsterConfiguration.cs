using ApplicationLayer.Models;
using DomainLayer.Domain.Projects;
using DomainLayer.Domain.Tasks;
using Mapster;
using ProjectEntity = DomainLayer.Domain.Projects.Project;

namespace InfrastructureLayer.Mapping
{
    public static class MapsterConfiguration
    {
        public static void Register()
        {
            TypeAdapterConfig<ProjectBase, ProjectSender>.NewConfig()
                .Map(dest => dest.Type, src => ResolveProjectKind(src));

            TypeAdapterConfig<TaskBase, TaskSender>.NewConfig()
                .Map(dest => dest.TaskType, src => ResolveTaskKind(src))
                .Map(dest => dest.Status, src => src.Status)
                .Map(dest => dest.AssignedUserId, src => src.AssignedUserId)
                .Map(dest => dest.Environment, src => BugEnvironment(src))
                .Map(dest => dest.Severity, src => BugSeverityOrNull(src))
                .Map(dest => dest.StoryPoints, src => FeatureStoryPointsOrNull(src));
        }

        private static ProjectType ResolveProjectKind(ProjectBase src)
        {
            return src switch
            {
                ProjectEntity => ProjectType.Standard,
                Holiday => ProjectType.Holiday,
                SickLeave => ProjectType.SickLeave,
                Training => ProjectType.Training,
                _ => throw new InvalidOperationException($"Unknown project subtype: {src.GetType().Name}"),
            };
        }

        private static TaskType ResolveTaskKind(TaskBase src)
        {
            return src switch
            {
                BugTask => TaskType.Bug,
                FeatureTask => TaskType.Feature,
                _ => throw new InvalidOperationException($"Unknown task subtype: {src.GetType().Name}"),
            };
        }

        private static string? BugEnvironment(TaskBase src) =>
            src is BugTask b ? b.Environment : null;

        private static BugSeverity? BugSeverityOrNull(TaskBase src) =>
            src is BugTask b ? b.Severity : null;

        private static int? FeatureStoryPointsOrNull(TaskBase src) =>
            src is FeatureTask f ? f.StoryPoints : null;
    }
}
