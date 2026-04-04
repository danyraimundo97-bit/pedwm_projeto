import 'package:graphql/client.dart';

import '../../models/hours_log_entry.dart';
import '../../models/project_model.dart';
import '../../models/task_model.dart';
import '../graphql/backend_maps.dart';
import '../graphql/graphql_operations.dart';
import '../graphql/graphql_result.dart';

const _kMockNetworkDelay = Duration(milliseconds: 400);

Future<List<MonthlyHoursStats>> fetchMonthlyHoursStatsFromBackend() async {
  await Future<void>.delayed(_kMockNetworkDelay);
  return [
    MonthlyHoursStats(
      month: DateTime.now(),
      projectCount: 2,
      taskCount: 4,
      totalHours: 20,
    ),
    MonthlyHoursStats(
      month: DateTime.now().subtract(const Duration(days: 30)),
      projectCount: 6,
      taskCount: 14,
      totalHours: 40,
    ),
    MonthlyHoursStats(
      month: DateTime.now().subtract(const Duration(days: 60)),
      projectCount: 6,
      taskCount: 10,
      totalHours: 40,
    ),
  ];
}

Future<List<TaskHoursInMonth>> fetchTaskHoursBreakdownForMonthFromBackend(DateTime month) async {
  await Future<void>.delayed(_kMockNetworkDelay);
  return [
    TaskHoursInMonth(projectTitle: 'Project 1', taskTitle: 'Task 1', hours: 10),
    TaskHoursInMonth(projectTitle: 'Project 2', taskTitle: 'Task 2', hours: 20),
    TaskHoursInMonth(projectTitle: 'Project 3', taskTitle: 'Task 3', hours: 30),
  ];
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
  final mgr = coerceGuid(managerId);
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
        'managerId': mgr,
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
  final projectsRaw = data?['getProjects'] as List<dynamic>? ?? [];
  final tasksRaw = data?['getTasks'] as List<dynamic>? ?? [];

  final byProject = <String, List<TaskModel>>{};
  for (final raw in tasksRaw) {
    final t = Map<String, dynamic>.from(raw as Map);
    final pid = t['projectId']?.toString();
    if (pid == null) continue;

    final kind = t['__typename']?.toString() ?? '';
    final taskType = kind.contains('Bug') ? TaskType.Bug : TaskType.Feature;

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
}) async {
  final result = await client.mutate(
    MutationOptions(
      document: GraphqlOperations.addHoursToProjectMutation,
      variables: {
        'input': {
          'projectId': projectId,
          'hours': hours,
        },
      },
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
