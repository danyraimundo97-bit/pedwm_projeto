import 'package:flutter/material.dart';
import '../models/project_model.dart';
import '../models/task_model.dart';

class ProjectProvider extends ChangeNotifier {
  List<ProjectModel> _projects = [];
  bool _isLoading = true;

  // Getters for the UI to consume
  List<ProjectModel> get projects => _projects;
  bool get isLoading => _isLoading;

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
        budgetHours: 50, consumedHours: 32, completionPercentage: 0,
        tasks: [
TaskModel(id: 't2', title: "Fix Login Crash", status: TaskStatus.fromString("ToDo"), type: TaskType.fromString("Bug"), estimate: 3, severity: "Critical"),
TaskModel(id: 't5', title: "Typo in Settings", status: TaskStatus.fromString("Active"), type: TaskType.fromString("Bug"), estimate: 1, severity: "Low"),
        ],
      ),
      ProjectModel(
        id: '2', 
        title: "Mobile App v2", 
        description: "Native features", 
        status: ProjectStatus.fromString("Active"), 
        budgetHours: 40, consumedHours: 18, completionPercentage: 0,
        tasks: [
          TaskModel(id: 't3', title: "Update Flutter SDK", status: TaskStatus.fromString("Completed"), type: TaskType.fromString("Feature"), estimate: 5),
          TaskModel(id: 't4', title: "Profile Image Upload", status: TaskStatus.fromString("Active"), type: TaskType.fromString("Feature"), estimate: 13),
        ],
      ),
   ];
    _isLoading = false;
    notifyListeners(); // Tells the UI: "Data is here, redraw!"
  }

  void updateTaskStatus(String projectId, String taskId, TaskStatus newStatus){
    final projectIndex = _projects.indexWhere((p) => p.id == projectId);
    if(projectIndex == -1) return;

    final project = _projects[projectIndex];

    final taskIndex = project.tasks.indexWhere((t) => t.id == taskId);
    if(taskIndex == -1) return;

    final updatedTask = project.tasks[taskIndex].copyWith(status: newStatus); 

    project.tasks[taskIndex] = updatedTask;

    // TO DO: fazer a chamada para o backend

    notifyListeners();
  }
}