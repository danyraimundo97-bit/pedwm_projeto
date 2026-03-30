using ApplicationLayer.Mapping;
using ApplicationLayer.Models;
using DomainLayer.Domain.Projects;
using DomainLayer.Domain.Tasks;
using Mapster;

namespace InfrastructureLayer.Mapping
{
    public sealed class DomainEntityDtoMapper : IDomainEntityDtoMapper
    {
        public ProjectSender ToProjectDto(ProjectBase project) => project.Adapt<ProjectSender>();

        public TaskSender ToTaskDto(TaskBase task) => task.Adapt<TaskSender>();
    }
}
