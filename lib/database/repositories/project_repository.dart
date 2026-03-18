import 'package:sqflite/sqflite.dart';

import '../../models/enums.dart';
import '../../models/project.dart';
import '../app_database.dart';

class ProjectRepository {
  Future<Database> get _db async => AppDatabase.instance.database;

  Future<List<Project>> listProjects() async {
    final db = await _db;
    final rows = await db.rawQuery('''
SELECT
  pb.id,
  pb.title,
  pb.type,
  pb.hours,
  p.budget_hours,
  p.client_name,
  p.status,
  p.manager_id,
  p.team_id
FROM project_base pb
JOIN projects p ON p.id = pb.id
ORDER BY pb.title ASC
''');

    return rows.map(Project.fromJoinedMap).toList();
  }

  Future<void> createProject(Project project) async {
    final db = await _db;

    await db.transaction((txn) async {
      await txn.insert('project_base', {
        'id': project.id,
        'title': project.title,
        'type': project.type,
        'hours': project.hours,
        'kind': ProjectKind.project.dbValue,
      });

      await txn.insert('projects', {
        'id': project.id,
        'budget_hours': project.budgetHours,
        'client_name': project.clientName,
        'status': project.status.dbValue,
        'manager_id': project.managerId,
        'team_id': project.teamId,
      });
    });
  }
}
