import 'package:graphql/client.dart';

import '../../models/hours_log_entry.dart';
import '../../models/project_model.dart';
import '../../models/task_model.dart';
import '../graphql/backend_maps.dart';
import '../graphql/graphql_operations.dart';
import '../graphql/graphql_result.dart';

class _MonthAgg {
  int totalHours = 0;
  final Set<String> projectIds = {};
  final Set<String> taskIds = {};
}

DateTime _monthStartUtc(DateTime d) {
  final u = d.toUtc();
  return DateTime.utc(u.year, u.month, 1);
}

DateTime _monthEndUtc(DateTime d) {
  final start = _monthStartUtc(d);
  return DateTime.utc(start.year, start.month + 1, 0, 23, 59, 59, 999);
}

Future<List<Map<String, dynamic>>> _fetchHourLogsRaw(
  GraphQLClient client,
  DateTime fromUtc,
  DateTime toUtc,
) async {
  final result = await client.query(
    QueryOptions(
      document: GraphqlOperations.hourLogsQuery,
      variables: {
        'from': fromUtc.toIso8601String(),
        'to': toUtc.toIso8601String(),
      },
      fetchPolicy: FetchPolicy.networkOnly,
    ),
  );
  assertNoGraphQlException(result);
  final raw = result.data?['hourLogs'] as List<dynamic>? ?? [];
  return raw.map((e) => Map<String, dynamic>.from(e as Map)).toList();
}

/// Last ~24 months of hour logs aggregated per calendar month (from `HourLogs` on the API).
Future<List<MonthlyHoursStats>> fetchMonthlyHoursStatsFromBackend(GraphQLClient client) async {
  final now = DateTime.now().toUtc();
  final from = DateTime.utc(now.year - 2, now.month, 1);
  final to = _monthEndUtc(now);
  final logs = await _fetchHourLogsRaw(client, from, to);

  final byMonth = <String, _MonthAgg>{};
  for (final log in logs) {
    final raw = log['loggedAtUtc'];
    if (raw == null) continue;
    final dt = DateTime.parse(raw.toString()).toUtc();
    final key = '${dt.year}-${dt.month.toString().padLeft(2, '0')}';
    final agg = byMonth.putIfAbsent(key, _MonthAgg.new);
    final h = (log['hours'] as num?)?.round() ?? 0;
    agg.totalHours += h;
    agg.projectIds.add(log['projectId'].toString());
    final tid = log['taskId'];
    if (tid != null) {
      agg.taskIds.add(tid.toString());
    }
  }

  final list = byMonth.entries.map((e) {
    final parts = e.key.split('-');
    final y = int.parse(parts[0]);
    final m = int.parse(parts[1]);
    final month = DateTime.utc(y, m, 1);
    final a = e.value;
    return MonthlyHoursStats(
      month: month,
      projectCount: a.projectIds.length,
      taskCount: a.taskIds.length,
      totalHours: a.totalHours,
    );
  }).toList();

  list.sort((a, b) => b.month.compareTo(a.month));
  return list;
}

Future<List<TaskHoursInMonth>> fetchTaskHoursBreakdownForMonthFromBackend(
  GraphQLClient client,
  DateTime month,
) async {
  final from = _monthStartUtc(month);
  final to = _monthEndUtc(month);
  final logs = await _fetchHourLogsRaw(client, from, to);
  final map = <String, int>{};
  for (final log in logs) {
    final pt = log['projectTitle']?.toString() ?? '';
    final tt = log['taskTitle']?.toString() ?? '';
    final key = '$pt\x00$tt';
    final h = (log['hours'] as num?)?.round() ?? 0;
    map[key] = (map[key] ?? 0) + h;
  }
  final list = map.entries
      .map(
        (e) {
          final parts = e.key.split('\x00');
          return TaskHoursInMonth(
            projectTitle: parts.isNotEmpty ? parts[0] : '',
            taskTitle: parts.length > 1 ? parts[1] : '',
            hours: e.value,
          );
        },
      )
      .toList();
  list.sort((a, b) => b.hours.compareTo(a.hours));
  return list;
}

Map<String, dynamic> _createProjectInput({
  required String title,
  required String description,
  required int budgetHours,
  required ProjectType type,
  required String managerId,
}) {
  final now = DateTime.now().toUtc();
  final end = now.add(const Duration(days: 365));
  final base = <String, dynamic>{
    'title': title,
    'startDate': now.toIso8601String(),
    'endDate': end.toIso8601String(),
    'allocatedHours': budgetHours.toDouble(),
    'type': BackendMaps.projectType(type),
  };
  switch (type) {
    case ProjectType.standard:
      return {
        ...base,
        'clientName': description,
        'managerId': managerId,
        'teamId': null,
      };
    case ProjectType.sickLeave:
      return {
        ...base,
        'medicalCertificateId': 'n/a',
        'isPaid': true,
      };
    case ProjectType.holiday:
      return {
        ...base,
        'holidayType': 'Optional',
      };
    case ProjectType.training:
      return {
        ...base,
        'courseName': title,
        'certificationLink': 'https://example.com',
      };
  }
}

List<ProjectModel> _parseProjectsAndTasks(Map<String, dynamic>? data) {
  final projectsRaw = data?['projects'] as List<dynamic>? ?? [];
  final tasksRaw = data?['tasks'] as List<dynamic>? ?? [];

  final byProject = <String, List<TaskModel>>{};
  for (final raw in tasksRaw) {
    final t = Map<String, dynamic>.from(raw as Map);
    final pid = t['projectId']?.toString();
    if (pid == null) continue;

    final taskType = BackendMaps.parseTaskType(
      t['taskType']?.toString() ?? t['__typename']?.toString(),
    );

    final estimate = taskType == TaskType.Feature
        ? ((t['storyPoints'] as num?)?.toInt() ?? 1)
        : 1;

    String? severity;
    if (t['severity'] != null) {
      severity = t['severity'].toString().split('.').last;
    }

    final assignee = t['assignedUserId']?.toString();

    final task = TaskModel(
      id: t['id'].toString(),
      title: t['title']?.toString() ?? '',
      description: t['description']?.toString() ?? '',
      status: BackendMaps.parseTaskStatus(t['status']?.toString()),
      type: taskType,
      estimate: estimate,
      loggedHours: 0,
      severity: severity,
      assigneeUserId: assignee == null || assignee.isEmpty ? null : assignee,
    );
    byProject.putIfAbsent(pid, () => []).add(task);
  }

  return projectsRaw.map((raw) {
    final p = Map<String, dynamic>.from(raw as Map);
    final id = p['id'].toString();
    final pt = BackendMaps.parseProjectType(p['type']?.toString());
    return ProjectModel(
      id: id,
      title: p['title']?.toString() ?? '',
      type: pt,
      description: '',
      budgetHours: 0,
      tasks: byProject[id] ?? [],
      status: ProjectStatus.Active,
      consumedHours: 0,
      completionPercentage: 0,
    );
  }).toList();
}

Future<List<ProjectModel>> fetchProjectsFromBackend(GraphQLClient client) async {
  final result = await client.query(
    QueryOptions(
      document: GraphqlOperations.projectsAndTasksQuery,
      fetchPolicy: FetchPolicy.networkOnly,
    ),
  );
  assertNoGraphQlException(result);
  return _parseProjectsAndTasks(result.data);
}

Future<void> createProjectInBackend(
  GraphQLClient client, {
  required String title,
  required String description,
  required int budgetHours,
  required ProjectType type,
  required String managerId,
}) async {
  final input = _createProjectInput(
    title: title,
    description: description,
    budgetHours: budgetHours,
    type: type,
    managerId: managerId,
  );
  final result = await client.mutate(
    MutationOptions(
      document: GraphqlOperations.createProjectMutation,
      variables: {'input': input},
    ),
  );
  assertNoGraphQlException(result);
}

Future<void> addHoursToProjectInBackend(
  GraphQLClient client, {
  required String projectId,
  required double hours,
  String? taskId,
}) async {
  final input = <String, dynamic>{
    'projectId': projectId,
    'hours': hours,
  };
  if (taskId != null && taskId.isNotEmpty) {
    input['taskId'] = taskId;
  }
  final result = await client.mutate(
    MutationOptions(
      document: GraphqlOperations.addHoursToProjectMutation,
      variables: {'input': input},
    ),
  );
  assertNoGraphQlException(result);
}

Future<void> changeProjectStatusInBackend(
  GraphQLClient client, {
  required String projectId,
  required ProjectStatus status,
}) async {
  final result = await client.mutate(
    MutationOptions(
      document: GraphqlOperations.changeProjectStatusMutation,
      variables: {
        'input': {
          'projectId': projectId,
          'status': BackendMaps.projectStatusForMutation(status),
        },
      },
    ),
  );
  assertNoGraphQlException(result);
}
