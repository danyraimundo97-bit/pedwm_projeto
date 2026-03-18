import 'enums.dart';

class Team {
  final String id;
  final String name;
  final String? description;
  final String? department;

  Team({
    required this.id,
    required this.name,
    this.description,
    this.department,
  });

  Map<String, Object?> toMap() => {
        'id': id,
        'name': name,
        'description': description,
        'department': department,
      };

  factory Team.fromMap(Map<String, Object?> map) => Team(
        id: map['id'] as String,
        name: map['name'] as String,
        description: map['description'] as String?,
        department: map['department'] as String?,
      );
}

class AppUser {
  final String id;
  final String name;
  final String email;
  final String? teamId;
  final UserRole role;

  AppUser({
    required this.id,
    required this.name,
    required this.email,
    required this.role,
    this.teamId,
  });

  Map<String, Object?> toMap() => {
        'id': id,
        'name': name,
        'email': email,
        'team_id': teamId,
        'role': role.dbValue,
      };

  factory AppUser.fromMap(Map<String, Object?> map) => AppUser(
        id: map['id'] as String,
        name: map['name'] as String,
        email: map['email'] as String,
        teamId: map['team_id'] as String?,
        role: UserRoleDb.fromDb(map['role'] as String),
      );
}

class ProjectBase {
  final String id;
  final String title;
  final String type;
  final int hours;
  final ProjectKind kind;

  ProjectBase({
    required this.id,
    required this.title,
    required this.type,
    required this.hours,
    required this.kind,
  });

  Map<String, Object?> toMap() => {
        'id': id,
        'title': title,
        'type': type,
        'hours': hours,
        'kind': kind.dbValue,
      };

  factory ProjectBase.fromMap(Map<String, Object?> map) => ProjectBase(
        id: map['id'] as String,
        title: map['title'] as String,
        type: map['type'] as String,
        hours: map['hours'] as int,
        kind: ProjectKindDb.fromDb(map['kind'] as String),
      );
}

class ProjectDetails {
  final String id;
  final int budgetHours;
  final String clientName;
  final ProjectStatus status;
  final String? managerId;
  final String? teamId;

  ProjectDetails({
    required this.id,
    required this.budgetHours,
    required this.clientName,
    required this.status,
    this.managerId,
    this.teamId,
  });

  Map<String, Object?> toMap() => {
        'id': id,
        'budget_hours': budgetHours,
        'client_name': clientName,
        'status': status.dbValue,
        'manager_id': managerId,
        'team_id': teamId,
      };

  factory ProjectDetails.fromMap(Map<String, Object?> map) => ProjectDetails(
        id: map['id'] as String,
        budgetHours: map['budget_hours'] as int,
        clientName: map['client_name'] as String,
        status: ProjectStatusDb.fromDb(map['status'] as String),
        managerId: map['manager_id'] as String?,
        teamId: map['team_id'] as String?,
      );
}

class HolidayDetails {
  final String id;
  final HolidayType holidayType;

  HolidayDetails({required this.id, required this.holidayType});

  Map<String, Object?> toMap() => {
        'id': id,
        'holiday_type': holidayType.dbValue,
      };

  factory HolidayDetails.fromMap(Map<String, Object?> map) => HolidayDetails(
        id: map['id'] as String,
        holidayType: HolidayTypeDb.fromDb(map['holiday_type'] as String),
      );
}

class TrainingDetails {
  final String id;
  final String courseName;

  TrainingDetails({required this.id, required this.courseName});

  Map<String, Object?> toMap() => {
        'id': id,
        'course_name': courseName,
      };

  factory TrainingDetails.fromMap(Map<String, Object?> map) => TrainingDetails(
        id: map['id'] as String,
        courseName: map['course_name'] as String,
      );
}

class TimeEntry {
  final String id;
  final String projectId;
  final String userId;
  final int hours;
  final String? description;
  final DateTime timestamp;

  TimeEntry({
    required this.id,
    required this.projectId,
    required this.userId,
    required this.hours,
    required this.timestamp,
    this.description,
  });

  Map<String, Object?> toMap() => {
        'id': id,
        'project_id': projectId,
        'user_id': userId,
        'hours': hours,
        'description': description,
        'timestamp': timestamp.toUtc().toIso8601String(),
      };

  factory TimeEntry.fromMap(Map<String, Object?> map) => TimeEntry(
        id: map['id'] as String,
        projectId: map['project_id'] as String,
        userId: map['user_id'] as String,
        hours: map['hours'] as int,
        description: map['description'] as String?,
        timestamp: DateTime.parse(map['timestamp'] as String),
      );
}

class PresenceEvent {
  final String userId;
  final String projectId;
  final ActionType action;
  final DateTime timestamp;

  PresenceEvent({
    required this.userId,
    required this.projectId,
    required this.action,
    required this.timestamp,
  });

  Map<String, Object?> toMap() => {
        'user_id': userId,
        'project_id': projectId,
        'action': action.dbValue,
        'timestamp': timestamp.toUtc().toIso8601String(),
      };

  factory PresenceEvent.fromMap(Map<String, Object?> map) => PresenceEvent(
        userId: map['user_id'] as String,
        projectId: map['project_id'] as String,
        action: ActionTypeDb.fromDb(map['action'] as String),
        timestamp: DateTime.parse(map['timestamp'] as String),
      );
}

class TaskBase {
  final String id;
  final String title;
  final String? description;
  final ProjectStatus status;
  final DateTime createdAt;
  final DateTime? completedAt;
  final String? assigneeId;
  final String projectId;
  final TaskType taskType;

  TaskBase({
    required this.id,
    required this.title,
    required this.status,
    required this.createdAt,
    required this.projectId,
    required this.taskType,
    this.description,
    this.completedAt,
    this.assigneeId,
  });

  Map<String, Object?> toMap() => {
        'id': id,
        'title': title,
        'description': description,
        'status': status.dbValue,
        'created_at': createdAt.toUtc().toIso8601String(),
        'completed_at': completedAt?.toUtc().toIso8601String(),
        'assignee_id': assigneeId,
        'project_id': projectId,
        'task_type': taskType.dbValue,
      };

  factory TaskBase.fromMap(Map<String, Object?> map) => TaskBase(
        id: map['id'] as String,
        title: map['title'] as String,
        description: map['description'] as String?,
        status: ProjectStatusDb.fromDb(map['status'] as String),
        createdAt: DateTime.parse(map['created_at'] as String),
        completedAt: map['completed_at'] == null
            ? null
            : DateTime.parse(map['completed_at'] as String),
        assigneeId: map['assignee_id'] as String?,
        projectId: map['project_id'] as String,
        taskType: TaskTypeDb.fromDb(map['task_type'] as String),
      );
}

class BugTaskDetails {
  final String id;
  final String environment;
  final Severity severity;

  BugTaskDetails({
    required this.id,
    required this.environment,
    required this.severity,
  });

  Map<String, Object?> toMap() => {
        'id': id,
        'environment': environment,
        'severity': severity.dbValue,
      };

  factory BugTaskDetails.fromMap(Map<String, Object?> map) => BugTaskDetails(
        id: map['id'] as String,
        environment: map['environment'] as String,
        severity: SeverityDb.fromDb(map['severity'] as String),
      );
}

class FeatureTaskDetails {
  final String id;
  final int storyPoints;

  FeatureTaskDetails({required this.id, required this.storyPoints});

  Map<String, Object?> toMap() => {
        'id': id,
        'story_points': storyPoints,
      };

  factory FeatureTaskDetails.fromMap(Map<String, Object?> map) =>
      FeatureTaskDetails(
        id: map['id'] as String,
        storyPoints: map['story_points'] as int,
      );
}
