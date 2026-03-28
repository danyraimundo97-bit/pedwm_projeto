import 'package:flutter/foundation.dart';
import '../models/app_user.dart';
import '../models/user_role.dart';
import '../data/repositories/users_repository.dart' as user_repo;

/// Directory of users (assignee pickers, admin, team membership resolution).
class UsersProvider extends ChangeNotifier {
  List<AppUser> _users = [];

  List<AppUser> get users => List.unmodifiable(_users);

  UsersProvider() {
    fetchUsers();
  }

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
    await user_repo.createUserInBackend(name, email, role);
    notifyListeners();
  }
}
