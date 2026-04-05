import 'package:graphql/client.dart';

import '../../models/app_user.dart';
import '../../models/user_role.dart';
import '../graphql/backend_maps.dart';
import '../graphql/graphql_operations.dart';
import '../graphql/graphql_result.dart';

const _kMockNetworkDelay = Duration(milliseconds: 400);

Future<List<AppUser>> fetchUsersFromBackend() async {
  await Future<void>.delayed(_kMockNetworkDelay);
  return [
    AppUser(id: 'u1', name: 'Jay Majors', email: 'jay@example.com', role: UserRole.projectManager),
    AppUser(id: 'u2', name: 'Alex Dev', email: 'alex@example.com', role: UserRole.member),
    AppUser(id: 'u3', name: 'Sam Admin', email: 'sam@example.com', role: UserRole.admin),
  ];
}

Future<AppUser> fetchCurrentUserFromBackend() async {
  await Future<void>.delayed(_kMockNetworkDelay);
  //TODO: Fazer chamada Backend
  return AppUser(id: '139591f2-0e7e-4ccb-a089-63bb96c1617b', name: 'Jay Majors', email: 'jay@example.com', role: UserRole.admin);
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
