using ApplicationLayer.Models;
using DomainLayer.Domain.Projects;
using DomainLayer.Domain.Tasks;
using Mapster;
using ProjectEntity = DomainLayer.Domain.Projects.Project;
using AppProjectType = ApplicationLayer.Models.ProjectType;
using AppTaskType = ApplicationLayer.Models.TaskType;

namespace InfrastructureLayer.Mapping
{
    public static class MapsterConfiguration
    {
        public static void Register()
        {
            TypeAdapterConfig<ProjectBase, ProjectDto>.NewConfig()
                .Map(dest => dest.Type, src => ResolveProjectKind(src));

            TypeAdapterConfig<TaskBase, TaskDto>.NewConfig()
                .Map(dest => dest.TaskType, src => ResolveTaskKind(src));
        }

        private static AppProjectType ResolveProjectKind(ProjectBase src)
        {
            return src switch
            {
                ProjectEntity => AppProjectType.Standard,
                Holiday => AppProjectType.Holiday,
                SickLeave => AppProjectType.SickLeave,
                Training => AppProjectType.Training,
                _ => throw new InvalidOperationException($"Unknown project subtype: {src.GetType().Name}"),
            };
        }

        private static AppTaskType ResolveTaskKind(TaskBase src)
        {
            return src switch
            {
                BugTask => AppTaskType.Bug,
                FeatureTask => AppTaskType.Feature,
                _ => throw new InvalidOperationException($"Unknown task subtype: {src.GetType().Name}"),
            };
        }
    }
}
