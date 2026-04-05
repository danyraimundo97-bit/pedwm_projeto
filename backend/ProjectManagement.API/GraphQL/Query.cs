using ApplicationLayer.Models;
using ApplicationLayer.Queries;
using HotChocolate;

namespace PresentationLayer.GraphQL
{
    public class Query
    {
        public string BemVindo() => "API de Gestão de Projetos Online";

        public async Task<IReadOnlyList<ProjectResponse>> GetProjects([Service] ListProjectsQueryHandler handler)
        {
            return await handler.HandleAsync();
        }

        public async Task<IReadOnlyList<TaskResponse>> GetTasks([Service] ListTasksQueryHandler handler)
        {
            return await handler.HandleAsync();
        }

        public async Task<IReadOnlyList<UserResponse>> GetUsers([Service] ListUsersQueryHandler handler)
        {
            return await handler.HandleAsync();
        }

        public async Task<IReadOnlyList<HourLogResponse>> GetHourLogs(
            DateTime from,
            DateTime to,
            [Service] ListHourLogsQueryHandler handler)
        {
            return await handler.HandleAsync(from, to);
        }
    }
}
