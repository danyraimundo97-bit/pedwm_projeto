import 'package:graphql/client.dart';

import '../../models/app_user.dart';
import '../../models/user_role.dart';
import '../graphql/backend_maps.dart';
import '../graphql/graphql_operations.dart';
import '../graphql/graphql_result.dart';

const _kMockNetworkDelay = Duration(milliseconds: 400);

List<AppUser> _parseUsers(Map<String, dynamic>? data) {
  final usersRaw = data?['users'] as List<dynamic>? ?? [];
  return usersRaw.map((raw) {
    final u = Map<String, dynamic>.from(raw as Map);
    return AppUser(
      id: u['id'].toString(),
      name: u['name']?.toString() ?? '',
      email: u['email']?.toString() ?? '',
      role: BackendMaps.parseUserRole(u['role']?.toString()),
    );
  }).toList();
}

Future<List<AppUser>> fetchUsersFromBackend(GraphQLClient client) async {
  final result = await client.query(
    QueryOptions(
      document: GraphqlOperations.usersQuery,
      fetchPolicy: FetchPolicy.networkOnly,
    ),
  );
  assertNoGraphQlException(result);
  return _parseUsers(result.data);
}

Future<AppUser> fetchCurrentUserFromBackend() async {
  await Future<void>.delayed(_kMockNetworkDelay);
  //TODO: Fazer chamada Backend
  // Must match backend seeded [SuperUser.Id] so SignalR group `user-{id}` matches session notifications.
  return AppUser(
    id: '00000000-0000-0000-0000-000000000001',
    name: 'Jay Majors',
    email: 'jay@example.com',
    role: UserRole.admin,
  );
}

Future<void> createUserInBackend(
  GraphQLClient client, {
  required String name,
  required String email,
  required UserRole role,
}) async {
  final result = await client.mutate(
    MutationOptions(
      document: GraphqlOperations.createUserMutation,
      variables: {
        'input': {
          'name': name,
          'email': email,
          'role': BackendMaps.userRole(role),
          'teamId': null,
        },
      },
    ),
  );
  assertNoGraphQlException(result);
}
