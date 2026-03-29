using ApplicationLayer.Models;
using DomainLayer.Domain.Projects;
using DomainLayer.Domain.Tasks;

namespace ApplicationLayer.Mapping
{
    /// <summary>Inbound mapping from domain entities to application DTOs (implemented in Infrastructure via Mapster).</summary>
    public interface IDomainEntityDtoMapper
    {
        ProjectDto ToProjectDto(ProjectBase project);

        TaskDto ToTaskDto(TaskBase task);
    }
}
