using ApplicationLayer.Mapping;
using ApplicationLayer.Models;
using DomainLayer.Domain.Projects;
using DomainLayer.Domain.Tasks;
using Mapster;

namespace InfrastructureLayer.Mapping
{
    public sealed class DomainEntityDtoMapper : IDomainEntityDtoMapper
    {
        public ProjectDto ToProjectDto(ProjectBase project) => project.Adapt<ProjectDto>();

        public TaskDto ToTaskDto(TaskBase task) => task.Adapt<TaskDto>();
    }
}
