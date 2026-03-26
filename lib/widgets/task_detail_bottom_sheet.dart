import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:provider/provider.dart';
import '../models/task_model.dart';
import '../providers/project_provider.dart';
import '../theme/app_colors.dart';

/// Opens the task detail sheet (description, log hours, status).
Future<void> showTaskDetailBottomSheet(
  BuildContext context, {
  required String projectId,
  required String taskId,
}) {
  return showModalBottomSheet<void>(
    context: context,
    isScrollControlled: true,
    backgroundColor: AppColors.cardBg,
    shape: const RoundedRectangleBorder(
      borderRadius: BorderRadius.vertical(top: Radius.circular(20)),
    ),
    builder: (context) => TaskDetailBottomSheet(
      projectId: projectId,
      taskId: taskId,
    ),
  );
}

class TaskDetailBottomSheet extends StatefulWidget {
  final String projectId;
  final String taskId;

  const TaskDetailBottomSheet({
    super.key,
    required this.projectId,
    required this.taskId,
  });

  @override
  State<TaskDetailBottomSheet> createState() => _TaskDetailBottomSheetState();
}

class _TaskDetailBottomSheetState extends State<TaskDetailBottomSheet> {
  late final TextEditingController _hoursController;

  @override
  void initState() {
    super.initState();
    _hoursController = TextEditingController();
  }

  @override
  void dispose() {
    _hoursController.dispose();
    super.dispose();
  }

  void _submitHours(BuildContext context) {
    final text = _hoursController.text.trim();
    final n = int.tryParse(text);
    if (n == null || n <= 0) return;
    context.read<ProjectProvider>().addLoggedHoursToTask(widget.projectId, widget.taskId, n);
    _hoursController.clear();
    FocusScope.of(context).unfocus();
  }

  @override
  Widget build(BuildContext context) {
    return Consumer<ProjectProvider>(
      builder: (context, provider, _) {
        final projectIndex = provider.projects.indexWhere((p) => p.id == widget.projectId);
        if (projectIndex == -1) return const SizedBox.shrink();
        final project = provider.projects[projectIndex];
        final taskIndex = project.tasks.indexWhere((t) => t.id == widget.taskId);
        if (taskIndex == -1) return const SizedBox.shrink();
        final task = project.tasks[taskIndex];
        final statuses = TaskStatus.labels;

        final bottomInset = MediaQuery.paddingOf(context).bottom;

        return SafeArea(
          child: SingleChildScrollView(
            padding: EdgeInsets.fromLTRB(20, 20, 20, 20 + bottomInset),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(
                  task.title,
                  style: const TextStyle(
                    color: AppColors.textPrimary,
                    fontSize: 18,
                    fontWeight: FontWeight.bold,
                  ),
                ),
                const SizedBox(height: 12),
                Text(
                  'Description',
                  style: TextStyle(
                    color: AppColors.textMuted,
                    fontSize: 12,
                    fontWeight: FontWeight.w600,
                    letterSpacing: 0.5,
                  ),
                ),
                const SizedBox(height: 6),
                Text(
                  task.description.trim().isEmpty ? 'No description.' : task.description,
                  style: const TextStyle(
                    color: AppColors.textSecondary,
                    fontSize: 14,
                    height: 1.4,
                  ),
                ),
                const SizedBox(height: 24),
                Text(
                  'Logged time',
                  style: TextStyle(
                    color: AppColors.textMuted,
                    fontSize: 12,
                    fontWeight: FontWeight.w600,
                    letterSpacing: 0.5,
                  ),
                ),
                const SizedBox(height: 8),
                Text(
                  '${task.loggedHours} h on this task · ${project.consumedHours} h total on project',
                  style: const TextStyle(color: AppColors.textSecondary, fontSize: 14),
                ),
                const SizedBox(height: 12),
                Row(
                  crossAxisAlignment: CrossAxisAlignment.end,
                  children: [
                    Expanded(
                      child: TextField(
                        controller: _hoursController,
                        keyboardType: TextInputType.number,
                        inputFormatters: [FilteringTextInputFormatter.digitsOnly],
                        style: const TextStyle(color: AppColors.textPrimary),
                        decoration: InputDecoration(
                          labelText: 'Add hours',
                          hintText: '0',
                          filled: true,
                          fillColor: AppColors.cardBgLighter,
                          border: OutlineInputBorder(
                            borderRadius: BorderRadius.circular(12),
                            borderSide: BorderSide.none,
                          ),
                          contentPadding: const EdgeInsets.symmetric(horizontal: 16, vertical: 12),
                        ),
                        onSubmitted: (_) => _submitHours(context),
                      ),
                    ),
                    const SizedBox(width: 12),
                    FilledButton(
                      onPressed: () => _submitHours(context),
                      child: const Text('Add'),
                    ),
                  ],
                ),
                const SizedBox(height: 24),
                Text(
                  'Status',
                  style: TextStyle(
                    color: AppColors.textMuted,
                    fontSize: 12,
                    fontWeight: FontWeight.w600,
                    letterSpacing: 0.5,
                  ),
                ),
                const SizedBox(height: 8),
                ...statuses.map((status) {
                  final isCurrent = task.status.label == status;
                  return ListTile(
                    contentPadding: EdgeInsets.zero,
                    title: Text(
                      status,
                      style: TextStyle(
                        color: isCurrent ? AppColors.accent : AppColors.textPrimary,
                        fontWeight: isCurrent ? FontWeight.bold : FontWeight.normal,
                      ),
                    ),
                    trailing: isCurrent ? const Icon(Icons.check_circle, color: AppColors.accent) : null,
                    onTap: () {
                      context.read<ProjectProvider>().updateTaskStatus(
                            widget.projectId,
                            widget.taskId,
                            TaskStatus.fromString(status),
                          );
                      Navigator.pop(context);
                    },
                  );
                }),
              ],
            ),
          ),
        );
      },
    );
  }
}
