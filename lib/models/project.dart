import 'enums.dart';

class Project {
  final String id;
  final String title;
  final String type;
  final int hours;
  final int budgetHours;
  final String clientName;
  final ProjectStatus status;
  final String? managerId;
  final String? teamId;

  Project({
    required this.id,
    required this.title,
    required this.type,
    required this.hours,
    required this.budgetHours,
    required this.clientName,
    required this.status,
    this.managerId,
    this.teamId,
  });

  factory Project.fromJoinedMap(Map<String, Object?> map) => Project(
        id: map['id'] as String,
        title: map['title'] as String,
        type: map['type'] as String,
        hours: map['hours'] as int,
        budgetHours: map['budget_hours'] as int,
        clientName: map['client_name'] as String,
        status: ProjectStatusDb.fromDb(map['status'] as String),
        managerId: map['manager_id'] as String?,
        teamId: map['team_id'] as String?,
      );
}
