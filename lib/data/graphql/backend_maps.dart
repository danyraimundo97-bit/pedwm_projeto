import '../../models/project_model.dart';
import '../../models/task_model.dart';
import '../../models/user_role.dart';

/// Maps Flutter UI enums to backend GraphQL enum names (Hot Chocolate: C# names, e.g. `Standard`, `Bug`).
class BackendMaps {
  BackendMaps._();

  static String projectType(ProjectType t) {
    switch (t) {
      case ProjectType.standar d:
        return 'Standard';
      case ProjectType.sickLeave:
        return 'SickLeave';
      case ProjectType.training:
        return 'Training';
      case ProjectType.holiday:
        return 'Holiday';
    }
  }

  static String taskType(TaskType t) {
    switch (t) {
      case TaskType.Feature:
        return 'Feature';
      case TaskType.Bug:
        return 'Bug';
    }
  }

  /// Bug severity for create task (Bug only). Flutter uses loose strings in UI.
  static String? bugSeverity(String? flutterSeverity) {
    if (flutterSeverity == null || flutterSeverity.isEmpty) return 'Medium';
    final s = flutterSeverity.toLowerCase();
    if (s == 'low') return 'Low';
    if (s == 'medium') return 'Medium';
    if (s == 'high') return 'High';
    if (s == 'critical') return 'Critical';
    return 'Medium';
  }

  static String userRole(UserRole role) {
    switch (role) {
      case UserRole.admin:
        return 'Admin';
      case UserRole.projectManager:
        return 'GP';
      case UserRole.member:
        return 'Standard';
    }
  }

  static String projectStatusForMutation(ProjectStatus status) {
    switch (status) {
      case ProjectStatus.Active:
        return 'Active';
      case ProjectStatus.Completed:
        return 'Completed';
      case ProjectStatus.OnHold:
        return 'OnHold';
      case ProjectStatus.ToDo:
        return 'Active';
      case ProjectStatus.Unknown:
        return 'Active';
    }
  }

  /// Backend [TaskStatus] → Flutter [TaskStatus].
  static TaskStatus parseTaskStatus(String? raw) {
    switch (raw) {
      case 'Todo':
        return TaskStatus.ToDo;
      case 'InProgress':
        return TaskStatus.Active;
      case 'InReview':
        return TaskStatus.Active;
      case 'Completed':
        return TaskStatus.Completed;
      default:
        return TaskStatus.Unknown;
    }
  }

  static ProjectType parseProjectType(String? raw) {
    if (raw == null || raw.isEmpty) return ProjectType.standard;
    return ProjectType.fromString(raw);
  }
}

/// Ensures a string is a valid UUID for GraphQL `UUID` scalars; uses [fallback] if not.
String coerceGuid(String? value, {String? fallback}) {
  const defaultGuid = 'f47ac10b-58cc-4372-a567-0e02b2c3d479';
  if (value == null || value.isEmpty) return fallback ?? defaultGuid;
  final re = RegExp(
    r'^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$',
  );
  if (re.hasMatch(value)) return value;
  return fallback ?? defaultGuid;
}
