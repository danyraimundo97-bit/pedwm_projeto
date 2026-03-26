import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
import 'package:provider/provider.dart';
import '../models/hours_log_entry.dart';
import '../providers/project_provider.dart';
import '../theme/app_colors.dart';

/// Monthly overview: tap a month to see tasks with hours logged.
class MonthlyHoursView extends StatefulWidget {
  const MonthlyHoursView({super.key});
  @override
  State<MonthlyHoursView> createState() => _MonthlyHoursViewState();
}
class _MonthlyHoursViewState extends State<MonthlyHoursView> {
  @override
  void initState() {
    super.initState();
    WidgetsBinding.instance.addPostFrameCallback((_) {
      if (!mounted) return;
      context.read<ProjectProvider>().loadMonthlyHoursStats();
    });
  }

  @override
  Widget build(BuildContext context) {
    final provider = context.watch<ProjectProvider>();
    final stats = provider.monthlyHoursStats;
    final monthFmt = DateFormat('MMMM yyyy', 'en_US');

    return Container(
      color: AppColors.background,
      child: SafeArea(
        top: false,
        child: CustomScrollView(
          slivers: [
            SliverToBoxAdapter(
              child: Padding(
                padding: const EdgeInsets.fromLTRB(20, 24, 20, 8),
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Text(
                      'Hours by month',
                      style: Theme.of(context).textTheme.headlineMedium?.copyWith(
                            fontWeight: FontWeight.bold,
                            color: AppColors.textPrimary,
                          ),
                    ),
                    const SizedBox(height: 4),
                    Text(
                      'Projects and tasks where you logged time — tap a month for details',
                      style: Theme.of(context).textTheme.bodyMedium?.copyWith(
                            color: AppColors.textSecondary,
                          ),
                    ),
                  ],
                ),
              ),
            ),
            if (provider.isLoading)
              const SliverFillRemaining(
                child: Center(
                  child: CircularProgressIndicator(color: AppColors.accent),
                ),
              )
            else if (stats.isEmpty)
              SliverFillRemaining(
                child: Center(
                  child: Text(
                    'No time entries yet.',
                    style: TextStyle(color: AppColors.textMuted),
                  ),
                ),
              )
            else
              SliverPadding(
                padding: const EdgeInsets.fromLTRB(20, 8, 20, 24),
                sliver: SliverList(
                  delegate: SliverChildBuilderDelegate(
                    (context, index) {
                      final row = stats[index];
                      final label = _capitalize(monthFmt.format(row.month));
                      return Padding(
                        padding: const EdgeInsets.only(bottom: 12),
                        child: _MonthCard(
                          label: label,
                          stats: row,
                          onTap: () => _openMonthTasksDialog(context, row.month, label),
                        ),
                      );
                    },
                    childCount: stats.length,
                  ),
                ),
              ),
          ],
        ),
      ),
    );
  }

  String _capitalize(String s) {
    if (s.isEmpty) return s;
    return s[0].toUpperCase() + s.substring(1);
  }

  Future<void> _openMonthTasksDialog(BuildContext context, DateTime month, String monthLabel) async {
    final provider = context.read<ProjectProvider>();
    await provider.loadTaskHoursBreakdownForMonth(month);
    if (!context.mounted) return;

    final rows = provider.taskHoursBreakdownForMonth;

    showDialog<void>(
      context: context,
      builder: (dialogContext) {
        return AlertDialog(
          backgroundColor: AppColors.cardBg,
          surfaceTintColor: Colors.transparent,
          title: Text(
            monthLabel,
            style: const TextStyle(color: AppColors.textPrimary, fontWeight: FontWeight.bold),
          ),
          content: SizedBox(
            width: double.maxFinite,
            child: rows.isEmpty
                ? const Text(
                    'No tasks for this month.',
                    style: TextStyle(color: AppColors.textSecondary),
                  )
                : ConstrainedBox(
                    constraints: const BoxConstraints(maxHeight: 360),
                    child: ListView.separated(
                      shrinkWrap: true,
                      itemCount: rows.length,
                      separatorBuilder: (context, index) => Divider(
                        height: 1,
                        color: AppColors.textMuted.withValues(alpha: 0.25),
                      ),
                      itemBuilder: (context, i) {
                        final r = rows[i];
                        return _TaskHoursRow(row: r);
                      },
                    ),
                  ),
          ),
          actions: [
            TextButton(
              onPressed: () => Navigator.of(dialogContext).pop(),
              child: const Text('Close'),
            ),
          ],
        );
      },
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

class _MonthCard extends StatelessWidget {
  final String label;
  final MonthlyHoursStats stats;
  final VoidCallback onTap;

  const _MonthCard({
    required this.label,
    required this.stats,
    required this.onTap,
  });

  @override
  Widget build(BuildContext context) {
    return Material(
      color: Colors.transparent,
      child: InkWell(
        onTap: onTap,
        borderRadius: BorderRadius.circular(16),
        child: Ink(
          padding: const EdgeInsets.all(18),
          decoration: BoxDecoration(
            color: AppColors.cardBg,
            borderRadius: BorderRadius.circular(16),
            border: Border.all(color: AppColors.cardBgLighter.withValues(alpha: 0.5)),
          ),
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Row(
                children: [
                  Container(
                    padding: const EdgeInsets.all(8),
                    decoration: BoxDecoration(
                      color: AppColors.accent.withValues(alpha: 0.2),
                      borderRadius: BorderRadius.circular(10),
                    ),
                    child: const Icon(Icons.calendar_month_rounded, color: AppColors.accent, size: 22),
                  ),
                  const SizedBox(width: 12),
                  Expanded(
                    child: Text(
                      label,
                      style: Theme.of(context).textTheme.titleMedium?.copyWith(
                            fontWeight: FontWeight.bold,
                            color: AppColors.textPrimary,
                          ),
                    ),
                  ),
                  Icon(Icons.chevron_right_rounded, color: AppColors.textMuted.withValues(alpha: 0.8)),
                ],
              ),
              const SizedBox(height: 14),
              Row(
                children: [
                  _MiniStat(
                    icon: Icons.folder_outlined,
                    value: '${stats.projectCount}',
                    caption: stats.projectCount == 1 ? 'project' : 'projects',
                  ),
                  const SizedBox(width: 20),
                  _MiniStat(
                    icon: Icons.task_alt_outlined,
                    value: '${stats.taskCount}',
                    caption: stats.taskCount == 1 ? 'task' : 'tasks',
                  ),
                  const Spacer(),
                  Text(
                    '${stats.totalHours} h',
                    style: Theme.of(context).textTheme.titleSmall?.copyWith(
                          color: AppColors.accent,
                          fontWeight: FontWeight.bold,
                        ),
                  ),
                ],
              ),
            ],
          ),
        ),
      ),
    );
  }
}

class _MiniStat extends StatelessWidget {
  final IconData icon;
  final String value;
  final String caption;

  const _MiniStat({
    required this.icon,
    required this.value,
    required this.caption,
  });

  @override
  Widget build(BuildContext context) {
    return Row(
      children: [
        Icon(icon, size: 18, color: AppColors.textMuted),
        const SizedBox(width: 6),
        Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Text(
              value,
              style: Theme.of(context).textTheme.titleMedium?.copyWith(
                    color: AppColors.textPrimary,
                    fontWeight: FontWeight.w700,
                  ),
            ),
            Text(
              caption,
              style: Theme.of(context).textTheme.labelSmall?.copyWith(
                    color: AppColors.textMuted,
                  ),
            ),
          ],
        ),
      ],
    );
  }
}
