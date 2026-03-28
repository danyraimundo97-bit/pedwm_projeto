import 'package:flutter/foundation.dart';
import '../models/app_user.dart';
import '../models/team_model.dart';
import '../models/user_role.dart';

/// Session + directory (replace with API + real auth).
class AuthProvider extends ChangeNotifier {
  // TODO: Remover o usuario mock e substituir pela API
  AppUser _currentUser = const AppUser(
    id: 'u1',
    name: 'Jay Majors',
    email: 'jay@example.com',
    role: UserRole.projectManager,
  );
// TODO: Remover os usuarios mock e substituir pela API
  final List<AppUser> _users = [
    const AppUser(id: 'u1', name: 'Jay Majors', email: 'jay@example.com', role: UserRole.projectManager),
    const AppUser(id: 'u2', name: 'Alex Dev', email: 'alex@example.com', role: UserRole.member),
    const AppUser(id: 'u3', name: 'Sam Admin', email: 'sam@example.com', role: UserRole.admin),
  ];

// TODO: Remover os teams mock e substituir pela API
  final List<TeamModel> _teams = [
    TeamModel(id: 'team1', name: 'Platform Squad', memberUserIds: ['u1', 'u2']),
  ];

  AppUser get currentUser => _currentUser;
  List<AppUser> get users => List.unmodifiable(_users);
  List<TeamModel> get teams => List.unmodifiable(_teams);

  bool get isAdmin => _currentUser.role == UserRole.admin;
  bool get isProjectManager => _currentUser.role == UserRole.projectManager;

  bool get canCreateUsers => isAdmin;
  bool get canCreateTeams => isAdmin;
  bool get canManageProjectsAndTasks => isAdmin || isProjectManager;
  bool get canAddTeamMembers => isAdmin || isProjectManager;

//DEV ONLY
//This function is used to set the role of the current user for the demo purposes
  void setDemoRole(UserRole role) {
    _currentUser = _currentUser.copyWith(role: role);
    notifyListeners();
  }

  void registerUser({
    required String name,
    required String email,
    UserRole role = UserRole.member,
  }) {
    final id = 'u${DateTime.now().millisecondsSinceEpoch}';
    _users.add(AppUser(id: id, name: name, email: email, role: role));
    dummyUpdateUsers();
    notifyListeners();
  }

  void createTeam(String name) {
    final id = 'team${DateTime.now().millisecondsSinceEpoch}';
    _teams.add(TeamModel(id: id, name: name));
    dummyUpdateTeams();
  }

  void addUserToTeam({required String teamId, required String userId}) {
    final i = _teams.indexWhere((t) => t.id == teamId);
    if (i == -1) return;
    final t = _teams[i];
    if (t.memberUserIds.contains(userId)) return;
    _teams[i] = TeamModel(
      id: t.id,
      name: t.name,
      memberUserIds: [...t.memberUserIds, userId],
    );
    dummyUpdateTeams();
  }

  void dummyUpdateUsers() {
    //TODO: Implement the API call to update the users
    notifyListeners();
  }

  void dummyUpdateTeams() {
    //TODO: Implement the API call to update the teams
    notifyListeners();
  }
}
