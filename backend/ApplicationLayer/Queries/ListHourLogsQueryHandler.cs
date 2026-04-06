using ApplicationLayer.Models;
using ApplicationLayer.Repositories;

namespace ApplicationLayer.Queries
{
    public sealed class ListHourLogsQueryHandler
    {
        private readonly IHourLogRepository _hourLogs;
        private readonly IProjectRepository _projects;
        private readonly ITaskRepository _tasks;

        public ListHourLogsQueryHandler(
            IHourLogRepository hourLogs,
            IProjectRepository projects,
            ITaskRepository tasks)
        {
            _hourLogs = hourLogs;
            _projects = projects;
            _tasks = tasks;
        }

        public async Task<IReadOnlyList<HourLogResponse>> HandleAsync(DateTime from, DateTime to)
        {
            var fromUtc = from.Kind == DateTimeKind.Utc ? from : from.ToUniversalTime();
            var toUtc = to.Kind == DateTimeKind.Utc ? to : to.ToUniversalTime();

            var logs = await _hourLogs.GetInRangeAsync(fromUtc, toUtc);
            var projectTitles = new Dictionary<Guid, string>();
            var taskTitles = new Dictionary<Guid, string>();

            var list = new List<HourLogResponse>(logs.Count);
            foreach (var log in logs)
            {
                if (!projectTitles.TryGetValue(log.ProjectId, out var projectTitle))
                {
                    var p = await _projects.GetByIdAsync(log.ProjectId);
                    projectTitle = p?.Title ?? "(unknown)";
                    projectTitles[log.ProjectId] = projectTitle;
                }

                string? taskTitle = null;
                if (log.TaskId is { } tid)
                {
                    if (!taskTitles.TryGetValue(tid, out var tt))
                    {
                        var t = await _tasks.GetByIdAsync(tid);
                        tt = t?.Title ?? string.Empty;
                        taskTitles[tid] = tt;
                    }
                    taskTitle = string.IsNullOrEmpty(tt) ? null : tt;
                }

                list.Add(new HourLogResponse
                {
                    Id = log.Id,
                    ProjectId = log.ProjectId,
                    ProjectTitle = projectTitle,
                    TaskId = log.TaskId,
                    TaskTitle = taskTitle,
                    Hours = log.Hours,
                    LoggedAtUtc = log.LoggedAtUtc,
                });
            }

            return list;
        }
    }
}
