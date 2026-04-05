import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

import '../providers/auth_provider.dart';
import '../providers/notifications_provider.dart';

/// Starts the SignalR connection when shown and stops it when disposed (e.g. user signs out).
class NotificationSessionShell extends StatefulWidget {
  const NotificationSessionShell({super.key, required this.child});

  final Widget child;

  @override
  State<NotificationSessionShell> createState() =>
      _NotificationSessionShellState();
}

class _NotificationSessionShellState extends State<NotificationSessionShell> {
  NotificationsProvider? _notifications;

  @override
  void didChangeDependencies() {
    super.didChangeDependencies();
    _notifications ??= context.read<NotificationsProvider>();
  }

  @override
  void initState() {
    super.initState();
    WidgetsBinding.instance.addPostFrameCallback((_) {
      if (!mounted) {
        return;
      }
      final auth = context.read<AuthProvider>();
      context.read<NotificationsProvider>().connectForUser(auth.user.id);
    });
  }

  @override
  void dispose() {
    _notifications?.disconnect();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) => widget.child;
}
