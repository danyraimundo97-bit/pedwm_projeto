import '../../models/project_model.dart';
import '../../models/task_model.dart';
import '../../models/user_role.dart';

/// Maps Flutter UI enums to backend GraphQL enum names (Hot Chocolate: C# names, e.g. `Standard`, `Bug`).
class BackendMaps {
  BackendMaps._();

  static String projectType(ProjectType t) {
    switch (t) {
      case ProjectType.standard:
        return 'STANDARD';
      case ProjectType.sickLeave:
        return 'SICK_LEAVE';
      case ProjectType.training:
        return 'TRAINING';
      case ProjectType.holiday:
        return 'HOLIDAY';
    }
  }

  static String taskType(TaskType t) {
    switch (t) {
      case TaskType.Feature:
        return 'FEATURE';
      case TaskType.Bug:
        return 'BUG';
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
        return 'ADMIN';
      case UserRole.projectManager:
        return 'GP';
      case UserRole.member:
        return 'STANDARD';
    }
  }

  /// GraphQL [UserRole] / backend enum → Flutter [UserRole].
  static UserRole parseUserRole(String? raw) {
    if (raw == null || raw.isEmpty) return UserRole.member;
    final token = raw.split(RegExp(r'[.\s]')).last.toUpperCase();
    switch (token) {
      case 'ADMIN':
        return UserRole.admin;
      case 'GP':
        return UserRole.projectManager;
      case 'STANDARD':
        return UserRole.member;
      default:
        return UserRole.member;
    }
  }

  static String projectStatusForMutation(ProjectStatus status) {
    switch (status) {
      case ProjectStatus.Active:
        return 'ACTIVE';
      case ProjectStatus.Completed:
        return 'COMPLETED';
      case ProjectStatus.OnHold:
        return 'ON_HOLD';
      case ProjectStatus.ToDo:
        return 'ACTIVE';
      case ProjectStatus.Unknown:
        return 'ACTIVE';
    }
  }

  /// Backend [TaskStatus] → Flutter [TaskStatus].
  static TaskStatus parseTaskStatus(String? raw) {
    switch (raw) {
      case 'TODO':
        return TaskStatus.ToDo;
      case 'IN_PROGRESS':
        return TaskStatus.Active;
      case 'IN_REVIEW':
        return TaskStatus.Active;
      case 'COMPLETED':
        return TaskStatus.Completed;
      default:
        return TaskStatus.Unknown;
    }
  }

  static ProjectType parseProjectType(String? raw) {
    if (raw == null || raw.isEmpty) return ProjectType.standard;
    return ProjectType.fromString(raw);
  }

  /// GraphQL <c>taskType</c> enum (Bug / Feature).
  static TaskType parseTaskType(String? raw) {
    if (raw == null || raw.isEmpty) return TaskType.Feature;
    final s = raw.toLowerCase();
    if (s.contains('bug')) return TaskType.Bug;
    if (s.contains('feature')) return TaskType.Feature;
    return TaskType.fromString(raw);
  }
}

