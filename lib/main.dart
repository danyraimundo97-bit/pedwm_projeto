import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'screens/main_navigation_screen.dart';
import 'theme/app_colors.dart';
import 'providers/project_provider.dart';

void main() {
  runApp(
    MultiProvider(
      providers: [ChangeNotifierProvider(create: (_) => ProjectProvider())],
      child: const TimePlannerApp(),
    ),
  );
}

class TimePlannerApp extends StatelessWidget {
  const TimePlannerApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Time Planner',
      theme: ThemeData.dark().copyWith(
        useMaterial3: true,
        colorScheme: ColorScheme.dark(
          primary: AppColors.accent,
          surface: AppColors.background,
          onSurface: AppColors.textPrimary,
        ),
        scaffoldBackgroundColor: AppColors.background,
        navigationBarTheme: NavigationBarThemeData(
          backgroundColor: AppColors.cardBg,
          indicatorColor: AppColors.transparent,
          iconTheme: WidgetStateProperty.resolveWith((states) {
            if (states.contains(WidgetState.selected)) {
              return const IconThemeData(color: AppColors.navSelected);
            }
            return const IconThemeData(color: AppColors.navUnselected);
          }),
          labelTextStyle: WidgetStateProperty.resolveWith((states) {
            if (states.contains(WidgetState.selected)) {
              return const TextStyle(
                color: AppColors.navSelected,
                fontWeight: FontWeight.w600,
              );
            }
            return const TextStyle(color: AppColors.navUnselected);
          }),
        ),
      ),
      home: const MainNavigationScreen(),
    );
  }
}
