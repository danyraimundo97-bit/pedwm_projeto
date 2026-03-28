import 'package:flutter/material.dart';
import '../models/hours_log_entry.dart';
import '../theme/app_colors.dart';

/// Dialog body: loading, error, or task rows for one month.
class MonthTasksDialog extends StatelessWidget {
  final String monthLabel;
  final Future<List<TaskHoursInMonth>> future;

  const MonthTasksDialog({
    super.key,
    required this.monthLabel,
    required this.future,
  });

  @override
  Widget build(BuildContext context) {
    return AlertDialog(
      backgroundColor: AppColors.cardBg,
      surfaceTintColor: Colors.transparent,
      title: Text(
        monthLabel,
        style: const TextStyle(color: AppColors.textPrimary, fontWeight: FontWeight.bold),
      ),
      content: SizedBox(
        width: double.maxFinite,
        child: FutureBuilder<List<TaskHoursInMonth>>(
          future: future,
          builder: (context, snapshot) {
            if (snapshot.connectionState == ConnectionState.waiting) {
              return const Padding(
                padding: EdgeInsets.symmetric(vertical: 24),
                child: Center(
                  child: CircularProgressIndicator(color: AppColors.accent),
                ),
              );
            }
            if (snapshot.hasError) {
              return Text(
                snapshot.error.toString(),
                style: const TextStyle(color: AppColors.error),
              );
            }
            final rows = snapshot.data ?? const <TaskHoursInMonth>[];
            if (rows.isEmpty) {
              return const Text(
                'No tasks for this month.',
                style: TextStyle(color: AppColors.textSecondary),
              );
            }
            return ConstrainedBox(
              constraints: const BoxConstraints(maxHeight: 360),
              child: ListView.separated(
                shrinkWrap: true,
                itemCount: rows.length,
                separatorBuilder: (context, index) => Divider(
                  height: 1,
                  color: AppColors.textMuted.withValues(alpha: 0.25),
                ),
                itemBuilder: (context, i) => _TaskHoursRow(row: rows[i]),
              ),
            );
          },
        ),
      ),
      actions: [
        TextButton(
          onPressed: () => Navigator.of(context).pop(),
          child: const Text('Close'),
        ),
      ],
    );
  }
}

class _TaskHoursRow extends StatelessWidget {
  final TaskHoursInMonth row;

  const _TaskHoursRow({required this.row});

  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: const EdgeInsets.symmetric(vertical: 6),
      child: Row(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Expanded(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(
                  row.taskTitle,
                  style: const TextStyle(
                    color: AppColors.textPrimary,
                    fontWeight: FontWeight.w600,
                    fontSize: 14,
                  ),
                ),
                const SizedBox(height: 2),
                Text(
                  row.projectTitle,
                  style: const TextStyle(
                    color: AppColors.textMuted,
                    fontSize: 12,
                  ),
                ),
              ],
            ),
          ),
          const SizedBox(width: 8),
          Text(
            '${row.hours} h',
            style: const TextStyle(
              color: AppColors.accent,
              fontWeight: FontWeight.bold,
              fontSize: 14,
            ),
          ),
        ],
      ),
    );
  }
}
