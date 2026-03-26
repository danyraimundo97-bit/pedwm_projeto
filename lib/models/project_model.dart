import 'task_model.dart'; // Add this import


enum ProjectStatus {
  Active,
  Completed,
  OnHold,
  ToDo,
  Unknown;

  static ProjectStatus fromString(String? status) {
    if(status == null) {
      return ProjectStatus.Unknown;
    }
    return ProjectStatus.values.firstWhere(
      (e) => e.name.toLowerCase() == status.toLowerCase(),
      orElse: () => ProjectStatus.Unknown,
    );
  }

  String get label {
    switch (this) {
      case ProjectStatus.Active: return 'Active';
      case ProjectStatus.ToDo: return 'To Do';
      case ProjectStatus.Completed: return 'Completed';
      case ProjectStatus.OnHold: return 'On Hold';
      case ProjectStatus.Unknown: return 'Unknown';
    }
  }

}


class ProjectModel {
  final String id;
  final String title;
  final String description;
  final int budgetHours;
  final List<TaskModel> tasks;
  ProjectStatus status;
  double consumedHours;
  double completionPercentage;

  ProjectModel({
    required this.id,
    required this.title,
    required this.description,
    required this.budgetHours,
    required this.tasks,
    this.status = ProjectStatus.Unknown,
    this.completionPercentage = 0, // TODO: Mudar depois para final quando tiver a chamada para o backend
    this.consumedHours = 0, // TODO: Mudar depois para final quando tiver a chamada para o backend
  });
}