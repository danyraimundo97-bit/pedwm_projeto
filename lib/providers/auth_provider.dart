import 'package:flutter/foundation.dart';
import '../models/app_user.dart';
import '../models/user_role.dart';
import '../data/repositories/users_repository.dart' as user_repo;

/// Session: current user, sign-in/out, permissions derived from [currentUser].
class AuthProvider extends ChangeNotifier {
  AppUser? _currentUser;

  /// True while the initial session / [fetchCurrentUser] is in progress.
  bool _isSessionLoading = true;

  /// Set when session fetch fails (e.g. network); shown on login screen.
  String? _sessionError;

  /// Current user when signed in; null before load completes or after sign-out / failed session.
  AppUser? get currentUser => _currentUser;

  /// Signed-in user. Only use when [isAuthenticated] is true (e.g. below the session gate).
  AppUser get user => _currentUser!;

  /// False until the first [fetchCurrentUser] attempt finishes (success or failure).
  bool get isSessionReady => !_isSessionLoading;

  /// True when [fetchCurrentUser] completed with a user. False if still loading, failed, or signed out.
  bool get isAuthenticated => _currentUser != null;

  String? get sessionError => _sessionError;

  bool get isAdmin => _currentUser?.role == UserRole.admin;
  bool get isProjectManager => _currentUser?.role == UserRole.projectManager;

  bool get canCreateUsers => isAdmin;
  bool get canCreateTeams => isAdmin;
  bool get canManageProjectsAndTasks => isAdmin || isProjectManager;
  bool get canAddTeamMembers => isAdmin || isProjectManager;

  AuthProvider() {
    fetchCurrentUser();
  }

//DEV ONLY
//This function is used to set the role of the current user for the demo purposes
  void setDemoRole(UserRole role) {
    final u = _currentUser;
    if (u == null) return;
    _currentUser = u.copyWith(role: role);
    notifyListeners();
  }

  /// Loads the current session from the backend (or clears it on failure).
  Future<void> fetchCurrentUser() async {
    _isSessionLoading = true;
    _sessionError = null;
    notifyListeners();
    try {
      _currentUser = await user_repo.fetchCurrentUserFromBackend();
    } catch (e, st) {
      _currentUser = null;
      _sessionError = e.toString();
      debugPrint('fetchCurrentUser: $e\n$st');
    } finally {
      _isSessionLoading = false;
      notifyListeners();
    }
  }

  void signOut() {
    _currentUser = null;
    _sessionError = null;
    notifyListeners();
  }
}
