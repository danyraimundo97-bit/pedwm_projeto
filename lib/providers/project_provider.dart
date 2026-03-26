import 'package:flutter/material.dart';
import '../models/hours_log_entry.dart';
import '../models/project_model.dart';
import '../models/task_model.dart';


class ProjectProvider extends ChangeNotifier {
  List<ProjectModel> _projects = [];
  final List<HoursLogEntry> _hoursLog = [];
  List<MonthlyHoursStats> _monthlyStats = [];
  List<TaskHoursInMonth> _taskHoursBreakdownForMonth = [];
  bool _isLoading = true;

  List<ProjectModel> get projects => _projects;
  bool get isLoading => _isLoading;

  //Retrieves the list of months with the hours logged in each month and sorts them by month descending (most recent first).
  List<MonthlyHoursStats> get monthlyHoursStats => _monthlyStats;
  //Retrieves the list of tasks with the hours logged in each month and sorts them by month descending (most recent first).
  List<TaskHoursInMonth> get taskHoursBreakdownForMonth => _taskHoursBreakdownForMonth;

  //Loads the list of months with the hours logged in each month and sorts them by month descending (most recent first).
  Future<void> loadMonthlyHoursStats() async {
    _isLoading = true;
    notifyListeners();
    _monthlyStats = await fetchMonthlyHoursStatsFromBackend();
    _isLoading = false;
    notifyListeners();
  }

  Future<List<MonthlyHoursStats>> fetchMonthlyHoursStatsFromBackend() async {
// TODO: mudar para ser uma chamada para o backend
    // Simulate network delay (2 seconds)
    await Future.delayed(const Duration(seconds: 2)); 
    return [
      MonthlyHoursStats(
        month: DateTime.now(),
        projectCount: 2,
        taskCount: 4,
        totalHours: 20,
      ),
      MonthlyHoursStats(
        month: DateTime.now().subtract(const Duration(days: 30)),
        projectCount: 6,
        taskCount: 14,
        totalHours: 40,
      ),
      MonthlyHoursStats(
        month: DateTime.now().subtract(const Duration(days: 60)),
        projectCount: 6,
        taskCount: 10,
        totalHours: 40,
      ),
    ];
  }

  /// Loads task breakdown for one month. Does not toggle [isLoading] so the
  /// monthly list is not replaced by a full-screen spinner while fetching.
  Future<void> loadTaskHoursBreakdownForMonth(DateTime month) async {
    _taskHoursBreakdownForMonth = await fetchTaskHoursBreakdownForMonthFromBackend(month);
    notifyListeners();
  }

  Future<List<TaskHoursInMonth>> fetchTaskHoursBreakdownForMonthFromBackend(DateTime month) async {

    await Future.delayed(const Duration(seconds: 2)); // TODO: mudar para ser uma chamada para o backend

    return [
      TaskHoursInMonth(
        projectTitle: "Project 1",
        taskTitle: "Task 1",
        hours: 10,
      ),
      TaskHoursInMonth(
        projectTitle: "Project 2",
        taskTitle: "Task 2",
        hours: 20,
      ),
      TaskHoursInMonth(
        projectTitle: "Project 3",
        taskTitle: "Task 3",
        hours: 30,
      ),
    ];
  }
  // Constructor automatically fetches data when the app starts
  ProjectProvider() {
    fetchProjectsFromBackend();
  }

  // Simulating a GraphQL fetch from your C# Backend
  Future<void> fetchProjectsFromBackend() async {
    _isLoading = true;
    notifyListeners(); // Tells the UI to show a loading spinner

    // Simulate network delay (2 seconds)
    await Future.delayed(const Duration(seconds: 2));

    // Dummy data acting as our GraphQL response
    _projects = [
      ProjectModel(
        id: '1',
        title: "Web Platform",
        description: "GraphQL Integration",
        status: ProjectStatus.fromString("Active"),
        budgetHours: 50,
        consumedHours: 0,
        completionPercentage: 0,
        tasks: [
          TaskModel(
            id: 't2',
            title: "Fix Login Crash",
            description:
                "App crashes on login when the session cookie is expired. Reproduce: cold start, tap Login with saved credentials.",
            status: TaskStatus.fromString("ToDo"),
            type: TaskType.fromString("Bug"),
            estimate: 3,
            loggedHours: 20,
            severity: "Critical",
          ),
          TaskModel(
            id: 't5',
            title: "Typo in Settings",
            description:
                "Settings > About shows 'Ver': fix label to 'Version'.",
            status: TaskStatus.fromString("Active"),
            type: TaskType.fromString("Bug"),
            estimate: 1,
            loggedHours: 12,
            severity: "Low",
          ),
        ],
      ),
      ProjectModel(
        id: '2',
        title: "Mobile App v2",
        description: "Native features",
        status: ProjectStatus.fromString("Active"),
        budgetHours: 40,
        completionPercentage: 0,
        tasks: [
          TaskModel(
            id: 't3',
            title: "Update Flutter SDK",
            description:
                "Bump to stable 3.x, run flutter pub upgrade, fix deprecations in android/ios configs.",
            status: TaskStatus.fromString("Completed"),
            type: TaskType.fromString("Feature"),
            estimate: 5,
            loggedHours: 10,
          ),
          TaskModel(
            id: 't4',
            title: "Profile Image Upload",
            description:
                "Allow picking from gallery or camera, crop to square, upload via GraphQL mutation with progress.",
            status: TaskStatus.fromString("Active"),
            type: TaskType.fromString("Feature"),
            estimate: 13,
            loggedHours: 8,
          ),
        ],
      ),
    ]; // TODO: mudar para ser uma chamada para o backend (timeEntries)
    _isLoading = false;
    notifyListeners(); // Tells the UI: "Data is here, redraw!"
  }


  void updateTaskStatus(String projectId, String taskId, TaskStatus newStatus) {
    final projectIndex = _projects.indexWhere((p) => p.id == projectId);
    if (projectIndex == -1) return;

    final project = _projects[projectIndex];
    //Isto é dummy para testar a UI, depois deve ser uma chamada para o backend
    final taskIndex = project.tasks.indexWhere((t) => t.id == taskId);
    if (taskIndex == -1) return;

    final updatedTask = project.tasks[taskIndex].copyWith(status: newStatus);

    project.tasks[taskIndex] = updatedTask;

    // TO DO: fazer a chamada para o backend

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

    // TODO: fazer a chamada para o backend
    dummyUpdateProjects();
    //fetchProjectsFromBackend();
    notifyListeners();
  }

  void dummyUpdateProjects() {
    _projects.forEach((project) {
      project.consumedHours = project.tasks.fold(0, (sum, task) => sum + task.loggedHours);
      project.completionPercentage = (project.consumedHours / project.budgetHours) * 100;
      if (project.completionPercentage > 100) {
        project.completionPercentage = 100;
      }
      if (project.completionPercentage < 0) {
        project.completionPercentage = 0;
      }
    });
  }
}
