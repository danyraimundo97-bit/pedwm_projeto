enum UserRole { admin, gp, standard }

enum ProjectStatus { active, completed, onHold }

enum HolidayType { fixed, optional }

enum ActionType { editing, viewing }

enum Severity { high, mid, low }

enum ProjectKind { project, holiday, training }

enum TaskType { bug, feature }

extension UserRoleDb on UserRole {
  String get dbValue => switch (this) {
        UserRole.admin => 'admin',
        UserRole.gp => 'gp',
        UserRole.standard => 'standard',
      };

  static UserRole fromDb(String value) => switch (value) {
        'admin' => UserRole.admin,
        'gp' => UserRole.gp,
        _ => UserRole.standard,
      };
}

extension ProjectStatusDb on ProjectStatus {
  String get dbValue => switch (this) {
        ProjectStatus.active => 'active',
        ProjectStatus.completed => 'completed',
        ProjectStatus.onHold => 'on_hold',
      };

  static ProjectStatus fromDb(String value) => switch (value) {
        'active' => ProjectStatus.active,
        'completed' => ProjectStatus.completed,
        _ => ProjectStatus.onHold,
      };
}

extension HolidayTypeDb on HolidayType {
  String get dbValue => switch (this) {
        HolidayType.fixed => 'fixed',
        HolidayType.optional => 'optional',
      };

  static HolidayType fromDb(String value) =>
      value == 'fixed' ? HolidayType.fixed : HolidayType.optional;
}

extension ActionTypeDb on ActionType {
  String get dbValue => switch (this) {
        ActionType.editing => 'editing',
        ActionType.viewing => 'viewing',
      };

  static ActionType fromDb(String value) =>
      value == 'editing' ? ActionType.editing : ActionType.viewing;
}

extension SeverityDb on Severity {
  String get dbValue => switch (this) {
        Severity.high => 'high',
        Severity.mid => 'mid',
        Severity.low => 'low',
      };

  static Severity fromDb(String value) => switch (value) {
        'high' => Severity.high,
        'mid' => Severity.mid,
        _ => Severity.low,
      };
}

extension ProjectKindDb on ProjectKind {
  String get dbValue => switch (this) {
        ProjectKind.project => 'project',
        ProjectKind.holiday => 'holiday',
        ProjectKind.training => 'training',
      };

  static ProjectKind fromDb(String value) => switch (value) {
        'project' => ProjectKind.project,
        'holiday' => ProjectKind.holiday,
        _ => ProjectKind.training,
      };
}

extension TaskTypeDb on TaskType {
  String get dbValue => switch (this) {
        TaskType.bug => 'bug',
        TaskType.feature => 'feature',
      };

  static TaskType fromDb(String value) =>
      value == 'bug' ? TaskType.bug : TaskType.feature;
}
