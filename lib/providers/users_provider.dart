import 'package:flutter/foundation.dart';
import 'package:graphql/client.dart';
import '../models/app_user.dart';
import '../models/user_role.dart';
import '../data/repositories/users_repository.dart' as user_repo;

/// Directory of users (assignee pickers, admin, team membership resolution).
class UsersProvider extends ChangeNotifier {
  UsersProvider(this._client) {
    fetchUsers();
  }

  final GraphQLClient _client;

  List<AppUser> _users = [];

  List<AppUser> get users => List.unmodifiable(_users);

  Future<void> fetchUsers() async {
    _users = await user_repo.fetchUsersFromBackend();
    notifyListeners();
  }

  Future<void> registerUser({
    required String name,
    required String email,
    UserRole role = UserRole.member,
  }) async {
    final id = 'u${DateTime.now().millisecondsSinceEpoch}';
    _users.add(AppUser(id: id, name: name, email: email, role: role));
    notifyListeners();
    try {
      await user_repo.createUserInBackend(_client, name: name, email: email, role: role);
      await fetchUsers();
    } catch (e, st) {
      debugPrint('createUserInBackend: $e\n$st');
      rethrow;
    }
  }
}
