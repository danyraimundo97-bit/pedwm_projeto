import 'package:flutter/foundation.dart';
import 'package:flutter/material.dart';

import 'database/app_database.dart';

Future<void> main() async {
  WidgetsFlutterBinding.ensureInitialized();
  if (_supportsSqlite()) {
    await AppDatabase.instance.database;
  }
  runApp(const TimePlannerApp());
}

bool _supportsSqlite() {
  if (kIsWeb) return false;
  return switch (defaultTargetPlatform) {
    TargetPlatform.android => true,
    TargetPlatform.iOS => true,
    TargetPlatform.macOS => true,
    TargetPlatform.linux => false,
    TargetPlatform.windows => false,
    TargetPlatform.fuchsia => false,
  };
}

class TimePlannerApp extends StatelessWidget {
  const TimePlannerApp({super.key});

  // This widget is the root of your application.
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Time Planner',
      theme: ThemeData(
        useMaterial3: true,
        colorScheme: ColorScheme.fromSeed(seedColor: Colors.indigo),
      ),
      home: const MainNavigationScreen(),
    );
  }
}

class DashboardView extends StatelessWidget {
  const DashboardView({super.key});

  @override
  Widget build(BuildContext context) {
    return const Center(
      child: Text('Dashboard'),
    );
  }
}

class ProjectListView extends StatelessWidget {
  const ProjectListView({super.key});

  @override
  Widget build(BuildContext context) {
    return const Center(
      child: Text('Projects'),
    );
  }
}

class ReportTimeView extends StatelessWidget {
  const ReportTimeView({super.key});

  @override
  Widget build(BuildContext context) {
    return const Center(
      child: Text('Report Time'),
    );
  }
}

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
    return Scaffold(
      appBar: AppBar(
        title: const Text("ESTG Time Planner"),
        backgroundColor: Theme.of(context).colorScheme.inversePrimary,
      ),
      body: _screens[_currentIndex],
      bottomNavigationBar: BottomNavigationBar(
        currentIndex: _currentIndex,
        onTap: (index) => setState(() => _currentIndex = index),
        items: const [
          BottomNavigationBarItem(icon: Icon(Icons.analytics), label: "Dashboard"),
          BottomNavigationBarItem(icon: Icon(Icons.folder), label: "Projects"),
          BottomNavigationBarItem(icon: Icon(Icons.add_task), label: "Report"),
        ],
      ),
    );
  }
}
