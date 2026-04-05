import 'package:flutter/foundation.dart';
import 'package:graphql/client.dart';
import '../models/hours_log_entry.dart';
import '../models/project_model.dart';
import '../models/task_model.dart';
import '../data/repositories/task_repository.dart';
import '../data/repositories/project_repository.dart';

class ProjectProvider extends ChangeNotifier {
  final GraphQLClient _client;

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

  bool get isLoadingProjects => _isLoadingProjects;

  bool get isLoadingMonthlyStats => _isLoadingMonthlyStats;

  String? get projectsError => _projectsError;

  String? get monthlyStatsError => _monthlyStatsError;

  List<MonthlyHoursStats> get monthlyHoursStats => _monthlyStats;
  List<TaskHoursInMonth> get taskHoursBreakdownForMonth => _taskHoursBreakdownForMonth;

  ProjectProvider(this._client) {
    fetchProjects();
  }

  Future<void> loadMonthlyHoursStats() async {
    _isLoadingMonthlyStats = true;
    _monthlyStatsError = null;
    notifyListeners();
    try {
      _monthlyStats = await fetchMonthlyHoursStatsFromBackend(_client);
    } catch (e, st) {
      _monthlyStatsError = _formatError(e);
      debugPrint('loadMonthlyHoursStats: $e\n$st');
    } finally {
      _isLoadingMonthlyStats = false;
      notifyListeners();
    }
  }

  Future<List<TaskHoursInMonth>> loadMonthDetailForDialog(DateTime month) async {
    final gen = ++_monthDetailGen;
    try {
      final list = await fetchTaskHoursBreakdownForMonthFromBackend(_client, month);
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
      _projects = await fetchProjectsFromBackend(_client);
      debugPrint('fetchProjectsFromBackend: $_projects');
      //dummyUpdateProjects();
    } catch (e, st) {
      _projectsError = _formatError(e);
      debugPrint('fetchProjectsFromBackend: $e\n$st');
    } finally {
      _isLoadingProjects = false;
      notifyListeners();
    }
  }

  Future<void> refreshProjects() async {
    await fetchProjects();
  }

  String _formatError(Object e) {
    if (e is Exception) return e.toString().replaceFirst('Exception: ', '');
    return e.toString();
  }

  Future<void> updateTaskStatus(String projectId, String taskId, TaskStatus newStatus) async {
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

  Future<void> updateTaskAssignee(String projectId, String taskId, String? assigneeUserId) async {
    final projectIndex = _projects.indexWhere((p) => p.id == projectId);
    if (projectIndex == -1) return;
    final project = _projects[projectIndex];
    final taskIndex = project.tasks.indexWhere((t) => t.id == taskId);
    if (taskIndex == -1) return;
    project.tasks[taskIndex] = project.tasks[taskIndex].copyWith(
      updateAssignee: true,
      assigneeUserId: assigneeUserId,
    );
    await updateTaskAssigneeInBackend(
      _client,
      projectId: projectId,
      taskId: taskId,
      assigneeUserId: assigneeUserId,
    );
    notifyListeners();
  }

  Future<void> addLoggedHoursToTask(String projectId, String taskId, int hours) async {
    if (hours <= 0) return;

    await addHoursToProjectInBackend(
      _client,
      projectId: projectId,
      hours: hours.toDouble(),
      taskId: taskId,
    );

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

  void dummyUpdateProjects() {
    for (final project in _projects) {
      project.consumedHours = project.tasks.fold<double>(0, (sum, task) => sum + task.loggedHours);
      final cap = project.budgetHours == 0 ? 1 : project.budgetHours;
      project.completionPercentage = (project.consumedHours / cap).clamp(0, 1);
    }
  }

  Future<void> addProject({
    required String title,
    required String description,
    required int budgetHours,
    required ProjectType type,
    required String managerUserId,
  }) async {
    //Removido para ir buscar pela BD
   /* final id = 'p${DateTime.now().millisecondsSinceEpoch}';
    _projects.add(
      ProjectModel(
        id: id,
        title: title,
        type: type,
        description: description,
        budgetHours: budgetHours,
        tasks: [],
        status: ProjectStatus.Active,
        consumedHours: 0,
        completionPercentage: 0,
      ),
    ); 
    notifyListeners();*/
    try {
      await createProjectInBackend(
        _client,
        title: title,
        description: description,
        budgetHours: budgetHours,
        type: type,
        managerId: managerUserId,
      );
      await fetchProjects();
      
    } catch (e, st) {
      _projectsError = _formatError(e);
      debugPrint('createProjectInBackend: $e\n$st');
    } finally{
      notifyListeners();
    }
  }

  Future<void> addTask({
    required String projectId,
    required String title,
    required String description,
    required int estimate,
    required TaskType type,
    String? severity,
    String? assigneeUserId,
  }) async {
    final projectIndex = _projects.indexWhere((p) => p.id == projectId);
    if (projectIndex == -1) return;
    /*final taskId = 't${DateTime.now().millisecondsSinceEpoch}';
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
    notifyListeners();*/
    try {
      await createTaskInBackend(
        _client,
        projectId: projectId,
        title: title,
        description: description,
        estimate: estimate,
        type: type,
        severity: severity,
        assigneeUserId: assigneeUserId,
      );
      await fetchProjects();
    } catch (e, st) {
      _projectsError = _formatError(e);
      debugPrint('createTaskInBackend: $e\n$st');
      notifyListeners();
    }
  }

  /// Persists status change for standard projects via GraphQL (optional UI use).
  Future<void> saveProjectStatusToBackend(String projectId, ProjectStatus status) async {
    await changeProjectStatusInBackend(_client, projectId: projectId, status: status);
    await fetchProjects();
  }
}
