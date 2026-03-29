import 'task_model.dart';

enum ProjectStatus {
  Active,
  Completed,
  OnHold,
  ToDo,
  Unknown;

  static ProjectStatus fromString(String? status) {
    if (status == null) {
      return ProjectStatus.Unknown;
    }
    return ProjectStatus.values.firstWhere(
      (e) => e.name.toLowerCase() == status.toLowerCase(),
      orElse: () => ProjectStatus.Unknown,
    );
  }

  String get label {
    switch (this) {
      case ProjectStatus.Active:
        return 'Active';
      case ProjectStatus.ToDo:
        return 'To Do';
      case ProjectStatus.Completed:
        return 'Completed';
      case ProjectStatus.OnHold:
        return 'On Hold';
      case ProjectStatus.Unknown:
        return 'Unknown';
    }
  }
}

/// Aligned with backend `DomainLayer.Domain.Projects.ProjectType`.
enum ProjectType {
  standard,
  sickLeave,
  training,
  holiday;

  static ProjectType fromString(String? value) {
    if (value == null || value.isEmpty) return ProjectType.standard;
    final v = value.trim();
    for (final e in ProjectType.values) {
      if (e.name.toLowerCase() == v.toLowerCase()) return e;
    }
    switch (v) {
      case 'Standard':
      case 'STANDARD':
        return ProjectType.standard;
      case 'SickLeave':
      case 'SICK_LEAVE':
        return ProjectType.sickLeave;
      case 'Training':
      case 'TRAINING':
        return ProjectType.training;
      case 'Holiday':
      case 'HOLIDAY':
        return ProjectType.holiday;
      default:
        return ProjectType.standard;
    }
  }

  /// Short label for chips and dropdowns.
  String get label {
    switch (this) {
      case ProjectType.standard:
        return 'Standard';
      case ProjectType.sickLeave:
        return 'Sick leave';
      case ProjectType.training:
        return 'Training';
      case ProjectType.holiday:
        return 'Holiday';
    }
  }
}

class ProjectModel {
  final String id;
  final String title;
  final ProjectType type;
  final String description;
  final int budgetHours;
  final List<TaskModel> tasks;
  ProjectStatus status;
  double consumedHours;
  double completionPercentage;

  ProjectModel({
    required this.id,
    required this.title,
    required this.type,
    required this.description,
    required this.budgetHours,
    required this.tasks,
    this.status = ProjectStatus.Unknown,
    this.completionPercentage = 0,
    this.consumedHours = 0,
  });
}
