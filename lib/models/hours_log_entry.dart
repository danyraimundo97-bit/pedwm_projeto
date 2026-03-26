/// One line of time logged on a task (used for monthly stats).
class HoursLogEntry {
  final String projectId;
  final String taskId;
  final int hours;
  final DateTime loggedAt;

  HoursLogEntry({
    required this.projectId,
    required this.taskId,
    required this.hours,
    required this.loggedAt,
  });
}

/// Distinct projects / tasks that had hours logged in a calendar month.
class MonthlyHoursStats {
  /// First day of the month (day normalized to 1).
  final DateTime month;
  final int projectCount;
  final int taskCount;
  final int totalHours;

  MonthlyHoursStats({
    required this.month,
    required this.projectCount,
    required this.taskCount,
    required this.totalHours,
  });
}

/// Aggregated hours per task within a month (for detail popup).
class TaskHoursInMonth {
  final String projectTitle;
  final String taskTitle;
  final int hours;

  TaskHoursInMonth({
    required this.projectTitle,
    required this.taskTitle,
    required this.hours,
  });
}
