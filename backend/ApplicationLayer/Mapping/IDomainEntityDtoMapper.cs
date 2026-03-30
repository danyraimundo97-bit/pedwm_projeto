using ApplicationLayer.Models;
using DomainLayer.Domain.Projects;
using DomainLayer.Domain.Tasks;
using DomainLayer.Domain.Users;
using DomainLayer.Domain.Teams;

namespace ApplicationLayer.Mapping
{
    /// <summary>Inbound mapping from domain entities to application DTOs (implemented in Infrastructure via Mapster).</summary>
    public interface IDomainEntityDtoMapper
    {
        ProjectDto ToProjectSender(ProjectBase project);
        TaskDto ToTaskDto(TaskBase task);
        UserResponse ToUserDto(User user);
        TeamSender ToTeamDto(Team team);
    }
}
