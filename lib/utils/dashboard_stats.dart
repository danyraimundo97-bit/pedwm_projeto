import '../models/project_model.dart';
import '../models/task_model.dart';
/// Aggregates for dashboard cards (computed from local project/task models).
class DashboardStats {
  final int activeProjects;
  final int tasksInProgress;
  final int tasksPending;
  final int tasksOnHold;
  final int tasksUnassigned;
  final int tasksCompleted;
  final int totalTasks;
  final int openBugs;
  final int projectsAtBudgetRisk;
  /// Share of tasks completed (0–1), or 0 if no tasks.
  final double completedTaskRatio;

  final int myActiveTasks;
  final int myPendingTasks;
  final int myOpenBugs;
  final int myLoggedHours;

  const DashboardStats({
    required this.activeProjects,
    required this.tasksInProgress,
    required this.tasksPending,
    required this.tasksOnHold,
    required this.tasksUnassigned,
    required this.tasksCompleted,
    required this.totalTasks,
    required this.openBugs,
    required this.projectsAtBudgetRisk,
    required this.completedTaskRatio,
    required this.myActiveTasks,
    required this.myPendingTasks,
    required this.myOpenBugs,
    required this.myLoggedHours,
  });

  /// Portfolio + per-user slices from [projects].
  factory DashboardStats.compute(
    List<ProjectModel> projects, {
    required String currentUserId,
  }) {
    final tasks = <TaskModel>[];
    for (final p in projects) {
      tasks.addAll(p.tasks);
    }

    bool isOpen(TaskModel t) =>
        t.status != TaskStatus.Completed && t.status != TaskStatus.Unknown;

    final activeProjects = projects.where((p) => p.status == ProjectStatus.Active).length;

    final tasksInProgress = tasks.where((t) => t.status == TaskStatus.Active).length;
    final tasksPending = tasks.where((t) => t.status == TaskStatus.ToDo).length;
    final tasksOnHold = tasks.where((t) => t.status == TaskStatus.OnHold).length;
    final tasksUnassigned =
        tasks.where((t) => t.assigneeUserId == null && isOpen(t)).length;
    final tasksCompleted = tasks.where((t) => t.status == TaskStatus.Completed).length;
    final totalTasks = tasks.length;

    final openBugs = tasks
        .where((t) => t.type == TaskType.Bug && isOpen(t))
        .length;

    final projectsAtBudgetRisk = projects.where((p) {
      if (p.budgetHours <= 0) return false;
      return (p.consumedHours / p.budgetHours) >= 0.9;
    }).length;

    final completedTaskRatio =
        totalTasks == 0 ? 0.0 : tasksCompleted / totalTasks;

    var myActive = 0;
    var myPending = 0;
    var myBugs = 0;
    var myHours = 0;
    for (final t in tasks) {
      if (t.assigneeUserId != currentUserId) continue;
      if (t.status == TaskStatus.Active) myActive++;
      if (t.status == TaskStatus.ToDo) myPending++;
      if (t.type == TaskType.Bug && isOpen(t)) myBugs++;
      myHours += t.loggedHours;
    }

    return DashboardStats(
      activeProjects: activeProjects,
      tasksInProgress: tasksInProgress,
      tasksPending: tasksPending,
      tasksOnHold: tasksOnHold,
      tasksUnassigned: tasksUnassigned,
      tasksCompleted: tasksCompleted,
      totalTasks: totalTasks,
      openBugs: openBugs,
      projectsAtBudgetRisk: projectsAtBudgetRisk,
      completedTaskRatio: completedTaskRatio,
      myActiveTasks: myActive,
      myPendingTasks: myPending,
      myOpenBugs: myBugs,
      myLoggedHours: myHours,
    );
  }

  /// Sum of in-progress + pending + on-hold (work queue size).
  int get openTasksCount => tasksInProgress + tasksPending + tasksOnHold;
}
