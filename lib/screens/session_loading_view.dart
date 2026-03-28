import 'package:flutter/material.dart';
import '../theme/app_colors.dart';

/// Shown while [AuthProvider.isSessionReady] is false.
class SessionLoadingView extends StatelessWidget {
  const SessionLoadingView({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppColors.background,
      body: Center(
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            const CircularProgressIndicator(color: AppColors.accent),
            const SizedBox(height: 24),
            Text(
              'Loading session…',
              style: Theme.of(context).textTheme.bodyLarge?.copyWith(color: AppColors.textSecondary),
            ),
          ],
        ),
      ),
    );
  }
}
