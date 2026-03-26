import 'package:flutter/material.dart';
import '../models/task_model.dart';
import '../theme/app_colors.dart';

/// Task row on the project details screen.
class ProjectTaskTile extends StatelessWidget {
  final TaskModel task;
  final VoidCallback onTap;

  const ProjectTaskTile({
    super.key,
    required this.task,
    required this.onTap,
  });

  @override
  Widget build(BuildContext context) {
    final isBug = task.type == TaskType.Bug;

    return Container(
      margin: const EdgeInsets.only(bottom: 12),
      decoration: BoxDecoration(
        color: AppColors.cardBg,
        borderRadius: BorderRadius.circular(12),
        border: Border.all(
          color: isBug ? AppColors.error.withValues(alpha: 0.3) : Colors.transparent,
        ),
      ),
      child: ListTile(
        onTap: onTap,
        contentPadding: const EdgeInsets.symmetric(horizontal: 16, vertical: 8),
        leading: Container(
          padding: const EdgeInsets.all(10),
          decoration: BoxDecoration(
            color: isBug ? AppColors.error.withValues(alpha: 0.2) : AppColors.accent.withValues(alpha: 0.2),
            borderRadius: BorderRadius.circular(8),
          ),
          child: Icon(
            isBug ? Icons.bug_report_rounded : Icons.code_rounded,
            color: isBug ? AppColors.error : AppColors.accent,
          ),
        ),
        title: Text(
          task.title,
          style: const TextStyle(color: AppColors.textPrimary, fontWeight: FontWeight.w600),
        ),
        subtitle: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Text(
              task.status.label,
              style: const TextStyle(color: AppColors.textSecondary, fontSize: 12),
            ),
            Text(
              '${task.loggedHours} h logged',
              style: const TextStyle(color: AppColors.textMuted, fontSize: 11),
            ),
            TaskSeverityTag(severity: task.severity),
          ],
        ),
        trailing: Container(
          padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 4),
          decoration: BoxDecoration(
            color: AppColors.cardBgLighter,
            borderRadius: BorderRadius.circular(12),
          ),
          child: Text(
            '${task.estimate} pts',
            style: const TextStyle(color: AppColors.textMuted, fontSize: 12, fontWeight: FontWeight.bold),
          ),
        ),
      ),
    );
  }
}

class TaskSeverityTag extends StatelessWidget {
  final String? severity;

  const TaskSeverityTag({super.key, required this.severity});

  @override
  Widget build(BuildContext context) {
    if (severity == null) return const SizedBox.shrink();

    return Container(
      margin: const EdgeInsets.only(top: 6),
      padding: const EdgeInsets.symmetric(horizontal: 8, vertical: 2),
      decoration: BoxDecoration(
        color: AppColors.error.withValues(alpha: 0.2),
        borderRadius: BorderRadius.circular(6),
      ),
      child: Text(
        severity!.toUpperCase(),
        style: const TextStyle(color: AppColors.error, fontSize: 10, fontWeight: FontWeight.bold),
      ),
    );
  }
}
