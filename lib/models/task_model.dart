enum TaskStatus {
  Active,
  Completed,
  OnHold,
  ToDo,
  Unknown;

  static TaskStatus fromString(String? status) {
    if (status == null) {
      return TaskStatus.Unknown;
    }

    return TaskStatus.values.firstWhere(
      (e) => e.name.toLowerCase() == status.toLowerCase(),
      orElse: () => TaskStatus.Unknown,
    );
  }

  String get label {
    switch (this) {
      case TaskStatus.Active:
        return 'Active';
      case TaskStatus.ToDo:
        return 'To Do';
      case TaskStatus.Completed:
        return 'Completed';
      case TaskStatus.OnHold:
        return 'On Hold';
      case TaskStatus.Unknown:
        return 'Unknown';
    }
  }

  static List<String> get labels {
    return ["Active", "To Do", "Completed", "On Hold", "Unknown"];
  }
}

enum TaskType {
  Feature,
  Bug;

  static TaskType fromString(String status) {
    return TaskType.values.firstWhere(
      (e) => e.name.toLowerCase() == status.toLowerCase(),
    );
  }
}

class TaskModel {
  final String id;
  final String title;
  final String description;
  final TaskStatus status;
  final TaskType type;
  final int estimate; // Story points or hours
  final int loggedHours;
  final String? severity;

  TaskModel({
    required this.id,
    required this.title,
    required this.description,
    required this.status,
    required this.type,
    required this.estimate,
    this.loggedHours = 0,
    this.severity,
  });

  TaskModel copyWith({
    String? id,
    String? title,
    String? description,
    TaskStatus? status,
    TaskType? type,
    int? estimate,
    int? loggedHours,
    String? severity,
  }) {
    return TaskModel(
      id: id ?? this.id,
      title: title ?? this.title,
      description: description ?? this.description,
      status: status ?? this.status,
      type: type ?? this.type,
      estimate: estimate ?? this.estimate,
      loggedHours: loggedHours ?? this.loggedHours,
      severity: severity ?? this.severity,
    );
  }
}
