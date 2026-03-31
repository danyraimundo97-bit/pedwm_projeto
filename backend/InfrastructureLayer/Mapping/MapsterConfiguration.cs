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
                .Map(dest => dest.TaskType, src => ResolveTaskKind(src));
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
    }
}
