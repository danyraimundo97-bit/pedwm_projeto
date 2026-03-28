import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import '../providers/auth_provider.dart';
import '../providers/project_provider.dart';
import '../theme/app_colors.dart';
import '../widgets/project_task_tile.dart';
import '../widgets/task_detail_bottom_sheet.dart';
import 'create_task_view.dart';
import 'team_members_view.dart';

class ProjectDetailsView extends StatelessWidget {
  final String projectId;

  const ProjectDetailsView({super.key, required this.projectId});

  @override
  Widget build(BuildContext context) {
    final provider = context.watch<ProjectProvider>();
    final auth = context.watch<AuthProvider>();

    final project = provider.projects.firstWhere(
      (p) => p.id == projectId,
      orElse: () => throw Exception('Project not found'),
    );

    return Scaffold(
      backgroundColor: AppColors.background,
      appBar: AppBar(
        backgroundColor: AppColors.background,
        elevation: 0,
        iconTheme: const IconThemeData(color: AppColors.textPrimary),
        title: Text(
          project.title,
          style: const TextStyle(color: AppColors.textPrimary, fontWeight: FontWeight.bold),
        ),
        actions: [
          if (auth.canAddTeamMembers)
            IconButton(
              tooltip: 'Teams & members',
              icon: const Icon(Icons.groups_outlined, color: AppColors.textPrimary),
              onPressed: () => Navigator.push<void>(
                    context,
                    MaterialPageRoute<void>(builder: (_) => const TeamMembersView()),
                  ),
            ),
        ],
      ),
      floatingActionButton: auth.canManageProjectsAndTasks
          ? FloatingActionButton.extended(
              onPressed: () => Navigator.push<void>(
                    context,
                    MaterialPageRoute<void>(
                      builder: (_) => CreateTaskView(projectId: project.id),
                    ),
                  ),
              icon: const Icon(Icons.add_task),
              label: const Text('Add task'),
            )
          : null,
      body: SafeArea(
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Padding(
              padding: const EdgeInsets.fromLTRB(20, 0, 20, 8),
              child: Row(
                children: [
                  Icon(Icons.schedule_rounded, size: 18, color: AppColors.textMuted),
                  const SizedBox(width: 8),
                  Text(
                    '${project.consumedHours} / ${project.budgetHours} h logged',
                    style: Theme.of(context).textTheme.bodyMedium?.copyWith(
                          color: AppColors.textSecondary,
                          fontWeight: FontWeight.w600,
                        ),
                  ),
                ],
              ),
            ),
            Padding(
              padding: const EdgeInsets.fromLTRB(20, 8, 20, 16),
              child: Text(
                'Tasks',
                style: Theme.of(context).textTheme.titleLarge?.copyWith(
                      color: AppColors.textPrimary,
                      fontWeight: FontWeight.bold,
                    ),
              ),
            ),
            Expanded(
              child: project.tasks.isEmpty
                  ? const Center(
                      child: Text(
                        'No tasks yet.',
                        style: TextStyle(color: AppColors.textMuted),
                      ),
                    )
                  : ListView.builder(
                      padding: const EdgeInsets.symmetric(horizontal: 20),
                      itemCount: project.tasks.length,
                      itemBuilder: (context, index) {
                        final task = project.tasks[index];
                        return ProjectTaskTile(
                          task: task,
                          onTap: () => showTaskDetailBottomSheet(
                            context,
                            projectId: project.id,
                            taskId: task.id,
                          ),
                        );
                      },
                    ),
            ),
          ],
        ),
      ),
    );
  }
}
