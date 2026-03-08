import 'package:flutter/material.dart';

void main() {
  runApp(const TimePlannerApp());
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
        colorScheme: .fromSeed(seedColor: Colors.indigo),
      ),
      home: const MainNavigationScreen(),
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