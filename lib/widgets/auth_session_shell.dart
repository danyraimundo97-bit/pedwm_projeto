import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import '../providers/auth_provider.dart';
import 'notification_session_shell.dart';
import '../screens/login_view.dart';
import '../screens/main_navigation_screen.dart';
import '../screens/session_loading_view.dart';

/// Shows loading, login, or the main app depending on [AuthProvider] session state.
class AuthSessionShell extends StatelessWidget {
  const AuthSessionShell({super.key});

  @override
  Widget build(BuildContext context) {
    return Consumer<AuthProvider>(
      builder: (context, auth, _) {
        if (!auth.isSessionReady) {
          return const SessionLoadingView();
        }
        if (!auth.isAuthenticated) {
          return const LoginView();
        }
        return const NotificationSessionShell(
          child: MainNavigationScreen(),
        );
      },
    );
  }
}
