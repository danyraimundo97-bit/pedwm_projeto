import 'package:flutter/material.dart';
import 'dashboard_view.dart';
import 'project_list_view.dart';
import 'report_time_view.dart';
import '../theme/app_colors.dart';

class MainNavigationScreen extends StatefulWidget {
  const MainNavigationScreen({super.key});

  @override
  State<MainNavigationScreen> createState() => _MainNavigationScreenState();
}

class _MainNavigationScreenState extends State<MainNavigationScreen> {
  int _currentIndex = 0;

  // These are our "Dummy" screens
  final List<Widget> _screens = [
    const DashboardView(),
    const ProjectListView(),
    const ReportTimeView(),
  ];

  @override
  Widget build(BuildContext context) {
    final isDashboard = _currentIndex == 0;
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
            label: "Projects"
          ),
          NavigationDestination(
            icon: Icon(Icons.add_task_outlined),
            selectedIcon: Icon(Icons.add_task),
            label: "Report")
        ],
      ),
    );
  }
}
