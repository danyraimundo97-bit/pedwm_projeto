import 'package:flutter/foundation.dart';
import 'package:flutter/material.dart';
import '../models/hours_log_entry.dart';
import '../models/project_model.dart';
import '../models/task_model.dart';
import '../data/repositories/task_repository.dart';
import '../data/repositories/project_repository.dart';

class ProjectProvider extends ChangeNotifier {
  List<ProjectModel> _projects = [];
  final List<HoursLogEntry> _hoursLog = [];
  List<MonthlyHoursStats> _monthlyStats = [];
  List<TaskHoursInMonth> _taskHoursBreakdownForMonth = [];

  bool _isLoadingProjects = true;
  bool _isLoadingMonthlyStats = false;
  int _monthDetailGen = 0;

  String? _projectsError;
  String? _monthlyStatsError;

  List<ProjectModel> get projects => _projects;

  /// Initial / projects tab loading.
  bool get isLoadingProjects => _isLoadingProjects;

  /// Hours-by-month tab loading.
  bool get isLoadingMonthlyStats => _isLoadingMonthlyStats;

  /// Last error from [fetchProjectsFromBackend] / [refreshProjects].
  String? get projectsError => _projectsError;

  /// Last error from [loadMonthlyHoursStats].
  String? get monthlyStatsError => _monthlyStatsError;

  List<MonthlyHoursStats> get monthlyHoursStats => _monthlyStats;
  List<TaskHoursInMonth> get taskHoursBreakdownForMonth => _taskHoursBreakdownForMonth;

  ProjectProvider() {
    fetchProjects();
  }

  Future<void> loadMonthlyHoursStats() async {
    _isLoadingMonthlyStats = true;
    _monthlyStatsError = null;
    notifyListeners();
    try {
      _monthlyStats = await fetchMonthlyHoursStatsFromBackend();
    } catch (e, st) {
      _monthlyStatsError = _formatError(e);
      debugPrint('loadMonthlyHoursStats: $e\n$st');
    } finally {
      _isLoadingMonthlyStats = false;
      notifyListeners();
    }
  }

  /// Loads month task rows for dialogs; ignores stale responses when [month] changes quickly.
  Future<List<TaskHoursInMonth>> loadMonthDetailForDialog(DateTime month) async {
    final gen = ++_monthDetailGen;
    try {
      final list = await fetchTaskHoursBreakdownForMonthFromBackend(month);
      if (gen != _monthDetailGen) {
        return const [];
      }
      _taskHoursBreakdownForMonth = list;
      notifyListeners();
      return list;
    } catch (e, st) {
      if (gen != _monthDetailGen) rethrow;
      debugPrint('loadMonthDetailForDialog: $e\n$st');
      rethrow;
    }
  }

  Future<void> fetchProjects() async {
    _isLoadingProjects = true;
    _projectsError = null;
    notifyListeners();
    try {
      _projects = await fetchProjectsFromBackend();
      dummyUpdateProjects();
    } catch (e, st) {
      _projectsError = _formatError(e);
      debugPrint('fetchProjectsFromBackend: $e\n$st');
    } finally {
      _isLoadingProjects = false;
      notifyListeners();
    }
  }

  /// Pull-to-refresh on Projects tab.
  Future<void> refreshProjects() async {
    await fetchProjectsFromBackend();
  }

  String _formatError(Object e) {
    if (e is Exception) return e.toString().replaceFirst('Exception: ', '');
    return e.toString();
  }

  void updateTaskStatus(String projectId, String taskId, TaskStatus newStatus) async {
    final projectIndex = _projects.indexWhere((p) => p.id == projectId);
    if (projectIndex == -1) return;

    final project = _projects[projectIndex];
    final taskIndex = project.tasks.indexWhere((t) => t.id == taskId);
    if (taskIndex == -1) return;

    final updatedTask = project.tasks[taskIndex].copyWith(status: newStatus);
    project.tasks[taskIndex] = updatedTask;
    await updateTaskStatusInBackend(projectId, taskId, newStatus);
    notifyListeners();
  }

  void updateTaskAssignee(String projectId, String taskId, String? assigneeUserId) async {
    final projectIndex = _projects.indexWhere((p) => p.id == projectId);
    if (projectIndex == -1) return;
    final project = _projects[projectIndex];
    final taskIndex = project.tasks.indexWhere((t) => t.id == taskId);
    if (taskIndex == -1) return;
    project.tasks[taskIndex] = project.tasks[taskIndex].copyWith(
      updateAssignee: true,
      assigneeUserId: assigneeUserId,
    );
    await updateTaskAssigneeInBackend(projectId, taskId, assigneeUserId);
    notifyListeners();
  }

  void addLoggedHoursToTask(String projectId, String taskId, int hours) {
    if (hours <= 0) return;

    final projectIndex = _projects.indexWhere((p) => p.id == projectId);
    if (projectIndex == -1) return;

    final project = _projects[projectIndex];
    final taskIndex = project.tasks.indexWhere((t) => t.id == taskId);
    if (taskIndex == -1) return;

    final task = project.tasks[taskIndex];
    final updated = task.copyWith(loggedHours: task.loggedHours + hours);
    project.tasks[taskIndex] = updated;

    _hoursLog.add(
      HoursLogEntry(
        projectId: projectId,
        taskId: taskId,
        hours: hours,
        loggedAt: DateTime.now(),
      ),
    );

    dummyUpdateProjects();
    notifyListeners();
  }

//remove this function when the API is implemented
  void dummyUpdateProjects() {
    for (final project in _projects) {
      project.consumedHours = project.tasks.fold<double>(0, (sum, task) => sum + task.loggedHours);
      final cap = project.budgetHours == 0 ? 1 : project.budgetHours;
      project.completionPercentage = (project.consumedHours / cap).clamp(0, 1);
    }
  }

  void addProject ({
    required String title,
    required String description,
    required int budgetHours,
  }) async {
    final id = 'p${DateTime.now().millisecondsSinceEpoch}';
    //TODO: Remove this logic when the API is implemented
    _projects.add(
      ProjectModel(
        id: id,
        title: title,
        description: description,
        budgetHours: budgetHours,
        tasks: [],
        status: ProjectStatus.Active,
        consumedHours: 0,
        completionPercentage: 0,
      ),
    );
    await createProjectInBackend(title, description, budgetHours);
    notifyListeners();
  }

  void addTask({
    required String projectId,
    required String title,
    required String description,
    required int estimate,
    required TaskType type,
    String? severity,
    String? assigneeUserId,
  }) async {
    //TODO: Remove this logic when the API is implemented
    final projectIndex = _projects.indexWhere((p) => p.id == projectId);
    if (projectIndex == -1) return;
    final taskId = 't${DateTime.now().millisecondsSinceEpoch}';
    final task = TaskModel(
      id: taskId,
      title: title,
      description: description,
      status: TaskStatus.ToDo,
      type: type,
      estimate: estimate,
      loggedHours: 0,
      severity: severity,
      assigneeUserId: assigneeUserId,
    );
    _projects[projectIndex].tasks.add(task);
    await createTaskInBackend(projectId, title, description, estimate, type, severity, assigneeUserId);
    notifyListeners();
  }
}
