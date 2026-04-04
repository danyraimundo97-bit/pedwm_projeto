import 'package:graphql/client.dart';

import '../../models/task_model.dart';
import '../graphql/backend_maps.dart';
import '../graphql/graphql_operations.dart';
import '../graphql/graphql_result.dart';

const _kMockNetworkDelay = Duration(milliseconds: 300);

/// Backend has no task-status mutation yet; keeps optimistic UI working offline-style.
Future<void> updateTaskStatusInBackend(String projectId, String taskId, TaskStatus newStatus) async {
  await Future<void>.delayed(_kMockNetworkDelay);
}

Future<void> updateTaskAssigneeInBackend(
  GraphQLClient client, {
  required String projectId,
  required String taskId,
  required String? assigneeUserId,
}) async {
  if (assigneeUserId == null || assigneeUserId.isEmpty) {
    return;
  }
  final result = await client.mutate(
    MutationOptions(
      document: GraphqlOperations.assignTaskToUserMutation,
      variables: {
        'input': {
          'projectId': projectId,
          'taskId': taskId,
          'assigneeUserId': coerceGuid(assigneeUserId),
        },
      },
    ),
  );
  assertNoGraphQlException(result);
}

Future<void> createTaskInBackend(
  GraphQLClient client, {
  required String projectId,
  required String title,
  required String description,
  required int estimate,
  required TaskType type,
  String? severity,
  String? assigneeUserId,
}) async {
  final pid = coerceGuid(projectId);
  final input = <String, dynamic>{
    'type': BackendMaps.taskType(type),
    'title': title,
    'description': description,
    'projectId': pid,
    'assignedUserId': assigneeUserId != null && assigneeUserId.isNotEmpty ? coerceGuid(assigneeUserId) : null,
  };

  if (type == TaskType.Bug) {
    input['environment'] = 'Production';
    input['severity'] = BackendMaps.bugSeverity(severity);
  } else {
    input['storyPoints'] = estimate;
  }

  final result = await client.mutate(
    MutationOptions(
      document: GraphqlOperations.createTaskMutation,
      variables: {'input': input},
    ),
  );
  assertNoGraphQlException(result);
}
