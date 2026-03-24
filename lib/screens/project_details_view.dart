import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import '../theme/app_colors.dart';
import '../providers/project_provider.dart';
import '../models/task_model.dart';

class ProjectDetailsView extends StatelessWidget {
  final String projectId; // We now take the ID!

  const ProjectDetailsView({super.key, required this.projectId});

  @override
  Widget build(BuildContext context) {
    // 1. Listen to the provider to get the latest data!
    final provider = context.watch<ProjectProvider>();
    
    // Find our specific project
    final project = provider.projects.firstWhere(
      (p) => p.id == projectId,
      orElse: () => provider.projects.first, // Fallback
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
      ),
      body: SafeArea(
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Padding(
              padding: const EdgeInsets.all(20.0),
              child: Text(
                "My Assigned Tasks",
                style: Theme.of(context).textTheme.titleLarge?.copyWith(
                      color: AppColors.textPrimary,
                      fontWeight: FontWeight.bold,
                    ),
              ),
            ),
            
            Expanded(
              child: project.tasks.isEmpty
                  ? const Center(child: Text("No tasks assigned yet.", style: TextStyle(color: AppColors.textMuted)))
                  : ListView.builder(
                      padding: const EdgeInsets.symmetric(horizontal: 20),
                      itemCount: project.tasks.length,
                      itemBuilder: (context, index) {
                        final task = project.tasks[index];
                        final isBug = task.type == TaskType.Bug;

                        return Container(
                          margin: const EdgeInsets.only(bottom: 12),
                          decoration: BoxDecoration(
                            color: AppColors.cardBg,
                            borderRadius: BorderRadius.circular(12),
                            border: Border.all(
                              color: isBug ? AppColors.error.withOpacity(0.3) : Colors.transparent,
                            ),
                          ),
                          
                          // 2. WE WRAP THE CONTENT IN A LIST TILE WITH ONTAP
                          child: ListTile(
                            onTap: () => _showStatusUpdateSheet(context, project.id, task),
                            contentPadding: const EdgeInsets.symmetric(horizontal: 16, vertical: 8),
                            leading: Container(
                              padding: const EdgeInsets.all(10),
                              decoration: BoxDecoration(
                                color: isBug ? AppColors.error.withOpacity(0.2) : AppColors.accent.withOpacity(0.2),
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
                                _buildSeverityTag(task.severity),
                              ],
                            ),
                            trailing: Container(
                              padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 4),
                              decoration: BoxDecoration(
                                color: AppColors.cardBgLighter,
                                borderRadius: BorderRadius.circular(12),
                              ),
                              child: Text(
                                "${task.estimate} pts",
                                style: const TextStyle(color: AppColors.textMuted, fontSize: 12, fontWeight: FontWeight.bold),
                              ),
                            ),
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

  // 3. THE MODERN BOTTOM SHEET METHOD
  void _showStatusUpdateSheet(BuildContext context, String projectId, TaskModel task) {
    final statuses = TaskStatus.labels;

    showModalBottomSheet(
      context: context,
      backgroundColor: AppColors.cardBg,
      shape: const RoundedRectangleBorder(
        borderRadius: BorderRadius.vertical(top: Radius.circular(20)),
      ),
      builder: (bottomSheetContext) {
        return SafeArea(
          child: Padding(
            padding: const EdgeInsets.symmetric(vertical: 20),
            child: Column(
              mainAxisSize: MainAxisSize.min, // Takes only as much space as it needs
              children: [
                const Text(
                  "Update Task Status",
                  style: TextStyle(color: AppColors.textPrimary, fontSize: 18, fontWeight: FontWeight.bold),
                ),
                const SizedBox(height: 16),
                
                // Map our statuses to clickable rows
                ...statuses.map((status) {
                  final isCurrent = task.status.label == status;
                  return ListTile(
                    title: Text(
                      status,
                      style: TextStyle(
                        color: isCurrent ? AppColors.accent : AppColors.textPrimary,
                        fontWeight: isCurrent ? FontWeight.bold : FontWeight.normal,
                      ),
                    ),
                    trailing: isCurrent ? const Icon(Icons.check_circle, color: AppColors.accent) : null,
                    onTap: () {
                      // Call our provider method!
                      context.read<ProjectProvider>().updateTaskStatus(projectId, task.id, TaskStatus.fromString(status));
                      
                      // Close the bottom sheet
                      Navigator.pop(bottomSheetContext);
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

  // Helper method from before
  Widget _buildSeverityTag(String? severity) {
    if (severity == null) return const SizedBox.shrink();
    
    // ... logic remains exactly the same as the previous step ...
    // (Pasting a shortened version here for brevity)
    return Container(
      margin: const EdgeInsets.only(top: 6),
      padding: const EdgeInsets.symmetric(horizontal: 8, vertical: 2),
      decoration: BoxDecoration(color: AppColors.error.withOpacity(0.2), borderRadius: BorderRadius.circular(6)),
      child: Text(severity.toUpperCase(), style: const TextStyle(color: AppColors.error, fontSize: 10, fontWeight: FontWeight.bold)),
    );
  }
}