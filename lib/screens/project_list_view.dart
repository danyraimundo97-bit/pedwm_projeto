import 'package:flutter/material.dart';
import 'package:pedwm_projeto/models/task_model.dart';
import 'package:provider/provider.dart'; // Importar o Provider
import '../theme/app_colors.dart';
import '../providers/project_provider.dart'; // Importar o teu Provider
import '../models/project_model.dart'; // Importar o Modelo
import 'project_details_view.dart';

class ProjectListView extends StatelessWidget {
  const ProjectListView({super.key});

  @override
  Widget build(BuildContext context) {
    final provider = context.watch<ProjectProvider>();

    return Container(
      color: AppColors.background,
      child: SafeArea(
        top: false,
        child: CustomScrollView(
          slivers: [
            SliverToBoxAdapter(
              child: Padding(
                padding: const EdgeInsets.fromLTRB(20, 24, 20, 16),
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Text(
                      "Projects",
                      style: Theme.of(context).textTheme.headlineMedium
                          ?.copyWith(
                            fontWeight: FontWeight.bold,
                            color: AppColors.textPrimary,
                          ),
                    ),
                    const SizedBox(height: 4),
                    Text(
                      "Your active projects",
                      style: Theme.of(context).textTheme.bodyMedium?.copyWith(
                        color: AppColors.textSecondary,
                      ),
                    ),
                  ],
                ),
              ),
            ),

            // 2. MOSTRAR LOADING, LISTA VAZIA, OU OS DADOS REAIS
            if (provider.isLoading)
              const SliverFillRemaining(
                child: Center(
                  child: CircularProgressIndicator(color: AppColors.accent),
                ),
              )
            else if (provider.projects.isEmpty)
              SliverFillRemaining(
                child: Center(
                  child: Text(
                    "No projects found",
                    style: TextStyle(color: AppColors.textMuted),
                  ),
                ),
              )
            else
              SliverPadding(
                padding: const EdgeInsets.fromLTRB(20, 0, 20, 24),
                sliver: SliverList(
                  delegate: SliverChildBuilderDelegate(
                    (context, index) {
                      // Ir buscar o projeto exato à lista
                      final project = provider.projects[index];
                      return Padding(
                        padding: const EdgeInsets.only(bottom: 16),
                        // Passar o modelo real para o cartão
                        child: _buildProjectCard(context, project),
                      );
                    },
                    childCount: provider
                        .projects
                        .length, // Usar o tamanho real da lista!
                  ),
                ),
              ),
          ],
        ),
      ),
    );
  }

  // 3. ATUALIZAR O CARTÃO PARA RECEBER O PROJECTMODEL
  Widget _buildProjectCard(BuildContext context, ProjectModel p) {
    return GestureDetector(
      onTap: () {
        // THIS IS THE NAVIGATION MAGIC!
        Navigator.push(
          context,
          MaterialPageRoute(
            builder: (context) => ProjectDetailsView(projectId: p.id,),
          ),
        );
      },
      child: Container(
        padding: const EdgeInsets.all(20),
        decoration: BoxDecoration(
          color: AppColors.cardBg,
          borderRadius: BorderRadius.circular(16),
        ),
        // ... the rest of your card UI remains exactly the same ...
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Row(
              children: [
                Container(
                  padding: const EdgeInsets.all(10),
                  decoration: BoxDecoration(
                    color: AppColors.accent.withOpacity(0.2),
                    borderRadius: BorderRadius.circular(12),
                  ),
                  child: const Icon(
                    Icons.folder_rounded,
                    color: AppColors.accent,
                    size: 24,
                  ),
                ),
                const SizedBox(width: 16),
                Expanded(
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Text(
                        p.title, // DADO REAL
                        style: Theme.of(context).textTheme.titleMedium
                            ?.copyWith(
                              fontWeight: FontWeight.bold,
                              color: AppColors.textPrimary,
                            ),
                        overflow: TextOverflow.ellipsis,
                      ),
                      const SizedBox(height: 4),
                      Text(
                        p.description, // DADO REAL
                        style: Theme.of(context).textTheme.bodySmall?.copyWith(
                          color: AppColors.textSecondary,
                        ),
                        maxLines: 2,
                        overflow: TextOverflow.ellipsis,
                      ),
                    ],
                  ),
                ),
                Container(
                  padding: const EdgeInsets.symmetric(
                    horizontal: 12,
                    vertical: 6,
                  ),
                  decoration: BoxDecoration(
                    color: AppColors.accent.withOpacity(0.2),
                    borderRadius: BorderRadius.circular(20),
                  ),
                  child: Text(
                    p.status.label,
                    style: Theme.of(context).textTheme.labelSmall?.copyWith(
                      color: AppColors.accent,
                      fontWeight: FontWeight.w600,
                    ),
                  ),
                ),
              ],
            ),
            const SizedBox(height: 20),
            Row(
              children: [
                Icon(
                  Icons.schedule_rounded,
                  size: 16,
                  color: AppColors.textMuted,
                ),
                const SizedBox(width: 8),
                Text(
                  "${p.tasks.where((t) => t.status == TaskStatus.Completed).length} / ${p.tasks.length} Tasks", // DADO REAL
                  style: Theme.of(context).textTheme.bodySmall?.copyWith(
                    color: AppColors.textSecondary,
                  ),
                ),
                const Spacer(),
                Text(
                  "${(p.completionPercentage * 100).round()}%", // DADO REAL
                  style: Theme.of(context).textTheme.labelMedium?.copyWith(
                    fontWeight: FontWeight.bold,
                    color: AppColors.accent,
                  ),
                ),
              ],
            ),
            const SizedBox(height: 10),
            ClipRRect(
              borderRadius: BorderRadius.circular(6),
              child: LinearProgressIndicator(
                value: p.completionPercentage, // DADO REAL
                minHeight: 8,
                backgroundColor: AppColors.cardBgLighter,
                valueColor: const AlwaysStoppedAnimation<Color>(
                  AppColors.accent,
                ),
              ),
            ),
          ],
        ),
      ),
    );
  }
}
