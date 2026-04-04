using ApplicationLayer.Mapping;
using ApplicationLayer.Models;
using DomainLayer.Domain.Projects;
using DomainLayer.Domain.Tasks;
using DomainLayer.Domain.Teams;
using DomainLayer.Domain.Users;
using Mapster;

namespace InfrastructureLayer.Mapping
{
    public sealed class DomainEntityDtoMapper : Mapper
    {
        public ProjectSender ToProjectSender(ProjectBase project) => project.Adapt<ProjectSender>();

        public TaskSender ToTaskSender(TaskBase task) => task.Adapt<TaskSender>();

        public UserResponse ToUserSender(User user) => user.Adapt<UserResponse>();

        public TeamSender ToTeamSender(Team team) => team.Adapt<TeamSender>();
    }
}
