using ApplicationLayer.Models;
using DomainLayer.Domain.Projects;
using DomainLayer.Domain.Tasks;
using DomainLayer.Domain.Users;
using DomainLayer.Domain.Teams;

namespace ApplicationLayer.Mapping
{
    /// <summary>Inbound mapping from domain entities to application DTOs (implemented in Infrastructure via Mapster).</summary>
    public interface Mapper
    {
        ProjectSender ToProjectSender(ProjectBase project);
        TaskSender ToTaskSender(TaskBase task);
        UserResponse ToUserSender(User user);
        TeamSender ToTeamSender(Team team);
    }
}
