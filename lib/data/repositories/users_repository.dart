import '../../models/app_user.dart';
import '../../models/user_role.dart';

const _kMockNetworkDelay = Duration(milliseconds: 1000);

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
  return AppUser(id: 'u1', name: 'Jay Majors', email: 'jay@example.com', role: UserRole.projectManager);
}

Future<void> createUserInBackend(String name, String email, UserRole role) async {
  await Future<void>.delayed(_kMockNetworkDelay);
}
