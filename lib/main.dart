import 'package:flutter/material.dart';
import 'package:intl/date_symbol_data_local.dart';
import 'package:provider/provider.dart';
import 'widgets/auth_session_shell.dart';
import 'theme/app_colors.dart';
import 'providers/auth_provider.dart';
import 'providers/project_provider.dart';
import 'providers/teams_provider.dart';
import 'providers/users_provider.dart';

Future<void> main() async {
  WidgetsFlutterBinding.ensureInitialized();
  await initializeDateFormatting('en_US', null);
  runApp(
    MultiProvider(
      providers: [
        ChangeNotifierProvider(create: (_) => AuthProvider()),
        ChangeNotifierProvider(create: (_) => UsersProvider()),
        ChangeNotifierProvider(create: (_) => TeamsProvider()),
        ChangeNotifierProvider(create: (_) => ProjectProvider()),
      ],
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
      home: const AuthSessionShell(),
    );
  }
}
