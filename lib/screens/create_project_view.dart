import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import '../providers/auth_provider.dart';
import '../providers/project_provider.dart';
import '../theme/app_colors.dart';

class CreateProjectView extends StatefulWidget {
  const CreateProjectView({super.key});

  @override
  State<CreateProjectView> createState() => _CreateProjectViewState();
}

class _CreateProjectViewState extends State<CreateProjectView> {
  final _title = TextEditingController();
  final _description = TextEditingController();
  final _budget = TextEditingController();

  @override
  void dispose() {
    _title.dispose();
    _description.dispose();
    _budget.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    final auth = context.watch<AuthProvider>();
    if (!auth.canManageProjectsAndTasks) {
      return Scaffold(
        backgroundColor: AppColors.background,
        appBar: AppBar(backgroundColor: AppColors.background, title: const Text('New project')),
        body: const Center(child: Text('You do not have permission to create projects.', style: TextStyle(color: AppColors.textMuted))),
      );
    }

    return Scaffold(
      backgroundColor: AppColors.background,
      appBar: AppBar(
        backgroundColor: AppColors.background,
        iconTheme: const IconThemeData(color: AppColors.textPrimary),
        title: const Text('New project', style: TextStyle(color: AppColors.textPrimary)),
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
              controller: _budget,
              style: const TextStyle(color: AppColors.textPrimary),
              keyboardType: TextInputType.number,
              decoration: _d('Budget (hours)'),
            ),
            const SizedBox(height: 24),
            FilledButton(
              onPressed: () {
                final title = _title.text.trim();
                final desc = _description.text.trim();
                final b = int.tryParse(_budget.text.trim()) ?? 0;
                if (title.isEmpty || b <= 0) return;
                context.read<ProjectProvider>().addProject(
                      title: title,
                      description: desc.isEmpty ? '—' : desc,
                      budgetHours: b,
                    );
                Navigator.pop(context);
              },
              child: const Text('Create project'),
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
