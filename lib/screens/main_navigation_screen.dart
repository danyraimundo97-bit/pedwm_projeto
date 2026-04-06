import 'dart:async';

import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

import '../models/push_notification.dart';
import '../providers/notifications_provider.dart';
import 'dashboard_view.dart';
import 'monthly_hours_view.dart';
import 'project_list_view.dart';
import '../theme/app_colors.dart';

class MainNavigationScreen extends StatefulWidget {
  const MainNavigationScreen({super.key});

  @override
  State<MainNavigationScreen> createState() => _MainNavigationScreenState();
}

class _MainNavigationScreenState extends State<MainNavigationScreen> {
  int _currentIndex = 0;

  StreamSubscription<PushNotification>? _notificationSub;

  final List<Widget> _screens = [
    const DashboardView(),
    const ProjectListView(),
    const MonthlyHoursView(),
  ];

  @override
  void initState() {
    super.initState();
    WidgetsBinding.instance.addPostFrameCallback((_) {
      if (!mounted) {
        return;
      }
      final provider = context.read<NotificationsProvider>();
      _notificationSub = provider.events.listen(_showNotificationSnack);
    });
  }

  void _showNotificationSnack(PushNotification n) {
    if (!mounted) {
      return;
    }
    ScaffoldMessenger.of(context).showSnackBar(
      SnackBar(content: Text(n.message)),
    );
  }

  @override
  void dispose() {
    unawaited(_notificationSub?.cancel());
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: _screens[_currentIndex],
        backgroundColor: AppColors.background,
        bottomNavigationBar: NavigationBar(
        backgroundColor: AppColors.cardBg,
        selectedIndex: _currentIndex,
        onDestinationSelected: (index) => setState(() => _currentIndex = index),
        destinations: const [
          NavigationDestination(
            icon: Icon(Icons.analytics_outlined),
            selectedIcon: Icon(Icons.analytics),
            label: "Dashboard",
          ),
          NavigationDestination(
            icon: Icon(Icons.folder_outlined),
            selectedIcon: Icon(Icons.folder),
            label: "Projects",
          ),
          NavigationDestination(
            icon: Icon(Icons.calendar_month_outlined),
            selectedIcon: Icon(Icons.calendar_month),
            label: "Hours",
          ),
        ],
      ),
    );
  }
}
