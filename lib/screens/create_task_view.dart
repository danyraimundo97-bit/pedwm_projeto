import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import '../models/task_model.dart';
import '../providers/auth_provider.dart';
import '../providers/project_provider.dart';
import '../providers/users_provider.dart';
import '../theme/app_colors.dart';

class CreateTaskView extends StatefulWidget {
  final String projectId;

  const CreateTaskView({super.key, required this.projectId});

  @override
  State<CreateTaskView> createState() => _CreateTaskViewState();
}

class _CreateTaskViewState extends State<CreateTaskView> {
  final _title = TextEditingController();
  final _description = TextEditingController();
  final _estimate = TextEditingController();
  TaskType _type = TaskType.Feature;
  String? _assigneeUserId;

  @override
  void dispose() {
    _title.dispose();
    _description.dispose();
    _estimate.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    final auth = context.watch<AuthProvider>();
    final users = context.watch<UsersProvider>();
    if (!auth.canManageProjectsAndTasks) {
      return Scaffold(
        backgroundColor: AppColors.background,
        appBar: AppBar(backgroundColor: AppColors.background, title: const Text('New task')),
        body: const Center(child: Text('You do not have permission to create tasks.', style: TextStyle(color: AppColors.textMuted))),
      );
    }

    return Scaffold(
      backgroundColor: AppColors.background,
      appBar: AppBar(
        backgroundColor: AppColors.background,
        iconTheme: const IconThemeData(color: AppColors.textPrimary),
        title: const Text('New task', style: TextStyle(color: AppColors.textPrimary)),
      ),
      body: SingleChildScrollView(
        padding: const EdgeInsets.all(20),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.stretch,
          children: [
            TextField(
              controller: _title,
              style: const TextStyle(color: AppColors.textPrimary),
              decoration: _d('Title'),
            ),
            const SizedBox(height: 12),
            TextField(
              controller: _description,
              style: const TextStyle(color: AppColors.textPrimary),
              maxLines: 3,
              decoration: _d('Description'),
            ),
            const SizedBox(height: 12),
            TextField(
              controller: _estimate,
              style: const TextStyle(color: AppColors.textPrimary),
              keyboardType: TextInputType.number,
              decoration: _d('Estimate (points)'),
            ),
            const SizedBox(height: 12),
            DropdownButtonFormField<TaskType>(
              value: _type,
              dropdownColor: AppColors.cardBg,
              style: const TextStyle(color: AppColors.textPrimary),
              decoration: _d('Type'),
              items: TaskType.values
                  .map((t) => DropdownMenuItem(value: t, child: Text(t.name)))
                  .toList(),
              onChanged: (v) => setState(() => _type = v ?? TaskType.Feature),
            ),
            const SizedBox(height: 12),
            DropdownButtonFormField<String?>(
              value: _assigneeUserId,
              dropdownColor: AppColors.cardBg,
              style: const TextStyle(color: AppColors.textPrimary),
              decoration: _d('Assignee'),
              items: [
                const DropdownMenuItem<String?>(
                  value: null,
                  child: Text('Unassigned'),
                ),
                ...users.users.map(
                  (u) => DropdownMenuItem<String?>(value: u.id, child: Text(u.name)),
                ),
              ],
              onChanged: (v) => setState(() => _assigneeUserId = v),
            ),
            const SizedBox(height: 24),
            FilledButton(
              onPressed: () {
                final title = _title.text.trim();
                final desc = _description.text.trim();
                final est = int.tryParse(_estimate.text.trim()) ?? 0;
                if (title.isEmpty || est <= 0) return;
                context.read<ProjectProvider>().addTask(
                      projectId: widget.projectId,
                      title: title,
                      description: desc.isEmpty ? '—' : desc,
                      estimate: est,
                      type: _type,
                      severity: _type == TaskType.Bug ? 'Medium' : null,
                      assigneeUserId: _assigneeUserId,
                    );
                Navigator.pop(context);
              },
              child: const Text('Create task'),
            ),
          ],
        ),
      ),
    );
  }

  InputDecoration _d(String label) => InputDecoration(
        labelText: label,
        labelStyle: const TextStyle(color: AppColors.textSecondary),
        filled: true,
        fillColor: AppColors.cardBgLighter,
        border: OutlineInputBorder(borderRadius: BorderRadius.circular(12), borderSide: BorderSide.none),
      );
}
