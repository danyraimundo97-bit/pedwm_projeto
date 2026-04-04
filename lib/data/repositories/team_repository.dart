import 'package:graphql/client.dart';

import '../../models/team_model.dart';
import '../graphql/backend_maps.dart';
import '../graphql/graphql_operations.dart';
import '../graphql/graphql_result.dart';

const _kMockNetworkDelay = Duration(milliseconds: 400);

Future<List<TeamModel>> fetchTeamsFromBackend() async {
  await Future<void>.delayed(_kMockNetworkDelay);
  return [
    TeamModel(id: 'team1', name: 'Platform Squad', memberUserIds: ['u1', 'u2']),
  ];
}

Future<void> createTeamInBackend(GraphQLClient client, {required String name}) async {
  final result = await client.mutate(
    MutationOptions(
      document: GraphqlOperations.createTeamMutation,
      variables: {
        'input': {'name': name},
      },
    ),
  );
  assertNoGraphQlException(result);
}

Future<void> addUserToTeamInBackend(
  GraphQLClient client, {
  required String teamId,
  required String userId,
}) async {
  final result = await client.mutate(
    MutationOptions(
      document: GraphqlOperations.assignUserToTeamMutation,
      variables: {
        'input': {
          'teamId': coerceGuid(teamId),
          'userId': coerceGuid(userId),
        },
      },
    ),
  );
  assertNoGraphQlException(result);
}
