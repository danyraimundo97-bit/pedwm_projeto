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
        public ProjectResponse ToProjectResponse(ProjectBase project) => project.Adapt<ProjectResponse>();

        public TaskResponse ToTaskResponse(TaskBase task) => task.Adapt<TaskResponse>();

        public UserResponse ToUserResponse(User user) => user.Adapt<UserResponse>();

        public TeamResponse ToTeamResponse(Team team) => team.Adapt<TeamResponse>();
    }
}
