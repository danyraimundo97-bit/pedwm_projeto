import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import '../models/project_model.dart';
import '../models/user_role.dart';
import '../providers/auth_provider.dart';
import '../providers/project_provider.dart';
import '../theme/app_colors.dart';
import '../utils/dashboard_stats.dart';
import '../widgets/dashboard_stat_card.dart';
import 'admin_panel_view.dart';
import 'team_members_view.dart';

class DashboardView extends StatelessWidget {
  const DashboardView({super.key});

  @override
  Widget build(BuildContext context) {
    final auth = context.watch<AuthProvider>();
    final projectProvider = context.watch<ProjectProvider>();
    final projects = projectProvider.projects;
    final role = auth.currentUser.role;
    final stats = DashboardStats.compute(
      projects,
      currentUserId: auth.currentUser.id,
    );

    return Container(
      color: AppColors.background,
      child: SafeArea(
        top: false,
        child: SingleChildScrollView(
          padding: const EdgeInsets.fromLTRB(20, 24, 20, 24),
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              _DashboardHeader.build(context),
              const SizedBox(height: 24),
              if (projectProvider.isLoadingProjects && projects.isEmpty)
                const Padding(
                  padding: EdgeInsets.symmetric(vertical: 48),
                  child: Center(child: CircularProgressIndicator(color: AppColors.accent)),
                )
              else if (role == UserRole.member)
                _MemberDashboard(stats: stats, projects: projects)
              else
                _PortfolioDashboard(
                  stats: stats,
                  projects: projects,
                  auth: auth,
                ),
            ],
          ),
        ),
      ),
    );
  }
}

class _DashboardHeader {
  static Widget build(BuildContext context) {
    final auth = context.watch<AuthProvider>();

    return Padding(
      padding: EdgeInsets.only(top: MediaQuery.of(context).padding.top + 8),
      child: Row(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Expanded(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(
                  auth.currentUser.name,
                  style: Theme.of(context).textTheme.headlineSmall?.copyWith(
                        fontWeight: FontWeight.bold,
                        color: AppColors.textPrimary,
                      ),
                ),
                const SizedBox(height: 4),
                Container(
                  padding: const EdgeInsets.symmetric(horizontal: 8, vertical: 4),
                  decoration: BoxDecoration(
                    color: AppColors.accent.withValues(alpha: 0.15),
                    borderRadius: BorderRadius.circular(8),
                  ),
                  child: Text(
                    auth.currentUser.role.label,
                    style: Theme.of(context).textTheme.labelSmall?.copyWith(
                          color: AppColors.accent,
                          fontWeight: FontWeight.w600,
                        ),
                  ),
                ),
              ],
            ),
          ),
          PopupMenuButton<String>(
            icon: Container(
              width: 44,
              height: 44,
              decoration: BoxDecoration(
                color: AppColors.cardBgLighter,
                shape: BoxShape.circle,
                border: Border.all(color: AppColors.textMuted, width: 1),
              ),
              child: const Icon(Icons.more_vert, color: AppColors.textSecondary, size: 22),
            ),
            color: AppColors.cardBg,
            onSelected: (value) {
              final auth = context.read<AuthProvider>();
              switch (value) {
                case 'admin':
                  Navigator.push(context, MaterialPageRoute<void>(builder: (_) => const AdminPanelView()));
                  break;
                case 'teams':
                  Navigator.push(context, MaterialPageRoute<void>(builder: (_) => const TeamMembersView()));
                  break;
                case 'role_admin':
                  auth.setDemoRole(UserRole.admin);
                  break;
                case 'role_pm':
                  auth.setDemoRole(UserRole.projectManager);
                  break;
                case 'role_member':
                  auth.setDemoRole(UserRole.member);
                  break;
              }
            },
            itemBuilder: (context) => [
              if (auth.canCreateUsers)
                const PopupMenuItem(value: 'admin', child: Text('Administration')),
              if (auth.canAddTeamMembers)
                const PopupMenuItem(value: 'teams', child: Text('Teams & members')),
              const PopupMenuDivider(),
              const PopupMenuItem(value: 'role_admin', child: Text('Demo: act as Admin')),
              const PopupMenuItem(value: 'role_pm', child: Text('Demo: act as Project Manager')),
              const PopupMenuItem(value: 'role_member', child: Text('Demo: act as Member')),
            ],
          ),
        ],
      ),
    );
  }
}

class _PortfolioDashboard extends StatelessWidget {
  final DashboardStats stats;
  final List<ProjectModel> projects;
  final AuthProvider auth;

  const _PortfolioDashboard({
    required this.stats,
    required this.projects,
    required this.auth,
  });

  @override
  Widget build(BuildContext context) {
    final ratio = stats.completedTaskRatio.clamp(0.0, 1.0);
    final sum = stats.tasksInProgress + stats.tasksPending + stats.tasksOnHold;
    final double fIn = sum == 0 ? 1.0 : stats.tasksInProgress / sum;
    final double fPe = sum == 0 ? 0.0 : stats.tasksPending / sum;
    final double fHo = sum == 0 ? 0.0 : stats.tasksOnHold / sum;

    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        _HeroCard(
          title: 'Tasks in progress',
          bigValue: '${stats.tasksInProgress}',
          subtitle: 'active tasks',
          badgeLabel: '${(ratio * 100).round()}%',
          badgeSubtitle: 'done (all tasks)',
          barFraction: ratio,
        ),
        const SizedBox(height: 16),
        Row(
          children: [
            Expanded(
              child: DashboardStatCard(
                title: 'Pending',
                subtitle: 'To Do / backlog',
                value: '${stats.tasksPending}',
                icon: Icons.pending_actions_outlined,
                accentColor: AppColors.warning,
              ),
            ),
            const SizedBox(width: 12),
            Expanded(
              child: DashboardStatCard(
                title: 'On hold',
                subtitle: 'Paused work',
                value: '${stats.tasksOnHold}',
                icon: Icons.pause_circle_outline,
                accentColor: AppColors.textMuted,
              ),
            ),
          ],
        ),
        const SizedBox(height: 12),
        Row(
          children: [
            Expanded(
              child: DashboardStatCard(
                title: 'Unassigned',
                subtitle: 'Open, no owner',
                value: '${stats.tasksUnassigned}',
                icon: Icons.person_off_outlined,
                accentColor: AppColors.caution,
              ),
            ),
            const SizedBox(width: 12),
            Expanded(
              child: DashboardStatCard(
                title: 'Budget risk',
                subtitle: '≥ 90% hours used',
                value: '${stats.projectsAtBudgetRisk}',
                icon: Icons.warning_amber_outlined,
                accentColor: stats.projectsAtBudgetRisk > 0 ? AppColors.error : AppColors.success,
              ),
            ),
          ],
        ),
        const SizedBox(height: 12),
        _OpenBugsRow(count: stats.openBugs),
        const SizedBox(height: 16),
        _WorkloadBar(
          label: 'Open work mix',
          flexInProgress: fIn,
          flexPending: fPe,
          flexOnHold: fHo,
        ),
        const SizedBox(height: 16),
        _ActiveProjectsCard(activeCount: stats.activeProjects),
        if (auth.isAdmin) ...[
          const SizedBox(height: 16),
          _AdminOrgCard(users: auth.users.length, teams: auth.teams.length),
        ],
        const SizedBox(height: 16),
        _ProjectsSection(
          projects: projects,
          title: 'Projects',
          subtitle: 'All active projects',
        ),
      ],
    );
  }
}

class _MemberDashboard extends StatelessWidget {
  final DashboardStats stats;
  final List<ProjectModel> projects;

  const _MemberDashboard({
    required this.stats,
    required this.projects,
  });

  @override
  Widget build(BuildContext context) {
    final uid = context.watch<AuthProvider>().currentUser.id;
    final mine = projects.where((p) => p.tasks.any((t) => t.assigneeUserId == uid)).toList();
    final myRatio = (stats.myActiveTasks + stats.myPendingTasks) == 0
        ? 0.0
        : stats.myActiveTasks / (stats.myActiveTasks + stats.myPendingTasks);

    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        _HeroCard(
          title: 'My active tasks',
          bigValue: '${stats.myActiveTasks}',
          subtitle: 'assigned to you',
          badgeLabel: '${stats.myLoggedHours} h',
          badgeSubtitle: 'logged on your tasks',
          barFraction: myRatio.clamp(0.0, 1.0),
        ),
        const SizedBox(height: 16),
        Row(
          children: [
            Expanded(
              child: DashboardStatCard(
                title: 'My backlog',
                subtitle: 'To Do assigned',
                value: '${stats.myPendingTasks}',
                icon: Icons.list_alt_outlined,
                accentColor: AppColors.accent,
              ),
            ),
            const SizedBox(width: 12),
            Expanded(
              child: DashboardStatCard(
                title: 'My bugs',
                subtitle: 'Open bugs on you',
                value: '${stats.myOpenBugs}',
                icon: Icons.bug_report_outlined,
                accentColor: AppColors.error,
              ),
            ),
          ],
        ),
        const SizedBox(height: 16),
        _ProjectsSection(
          projects: mine.isEmpty ? projects : mine,
          title: mine.isEmpty ? 'Projects' : 'Your projects',
          subtitle: mine.isEmpty ? 'No assignments yet — browse below' : 'Where you have assigned work',
        ),
      ],
    );
  }
}

class _HeroCard extends StatelessWidget {
  final String title;
  final String bigValue;
  final String subtitle;
  final String badgeLabel;
  final String badgeSubtitle;
  final double barFraction;

  const _HeroCard({
    required this.title,
    required this.bigValue,
    required this.subtitle,
    required this.badgeLabel,
    required this.badgeSubtitle,
    required this.barFraction,
  });

  @override
  Widget build(BuildContext context) {
    final h = (barFraction.clamp(0.0, 1.0)) * 80;
    return Container(
      padding: const EdgeInsets.all(20),
      decoration: BoxDecoration(
        color: AppColors.cardBg,
        borderRadius: BorderRadius.circular(16),
      ),
      child: Row(
        children: [
          Expanded(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(
                  title,
                  style: Theme.of(context).textTheme.titleMedium?.copyWith(color: AppColors.textSecondary),
                ),
                const SizedBox(height: 8),
                Text(
                  bigValue,
                  style: Theme.of(context).textTheme.displaySmall?.copyWith(
                        fontWeight: FontWeight.bold,
                        color: AppColors.textPrimary,
                      ),
                ),
                Text(
                  subtitle,
                  style: Theme.of(context).textTheme.bodyMedium?.copyWith(color: AppColors.textSecondary),
                ),
              ],
            ),
          ),
          const SizedBox(width: 16),
          Column(
            children: [
              Container(
                padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 6),
                decoration: BoxDecoration(
                  color: AppColors.accent,
                  borderRadius: BorderRadius.circular(8),
                ),
                child: Column(
                  children: [
                    Text(
                      badgeLabel,
                      style: Theme.of(context).textTheme.labelLarge?.copyWith(
                            fontWeight: FontWeight.bold,
                            color: AppColors.textPrimary,
                          ),
                    ),
                    Text(
                      badgeSubtitle,
                      style: Theme.of(context).textTheme.labelSmall?.copyWith(
                            color: AppColors.textPrimary.withValues(alpha: 0.85),
                            fontSize: 10,
                          ),
                    ),
                  ],
                ),
              ),
              const SizedBox(height: 12),
              SizedBox(
                width: 12,
                height: 80,
                child: Stack(
                  alignment: Alignment.bottomCenter,
                  children: [
                    Container(
                      width: 12,
                      height: 80,
                      decoration: BoxDecoration(
                        color: AppColors.cardBgLighter,
                        borderRadius: BorderRadius.circular(6),
                      ),
                    ),
                    Container(
                      width: 12,
                      height: h < 4 ? 4 : h,
                      decoration: BoxDecoration(
                        color: AppColors.accent,
                        borderRadius: BorderRadius.circular(6),
                      ),
                    ),
                  ],
                ),
              ),
            ],
          ),
        ],
      ),
    );
  }
}

class _OpenBugsRow extends StatelessWidget {
  final int count;

  const _OpenBugsRow({required this.count});

  @override
  Widget build(BuildContext context) {
    return Container(
      padding: const EdgeInsets.all(16),
      decoration: BoxDecoration(
        color: AppColors.cardBg,
        borderRadius: BorderRadius.circular(12),
        border: Border.all(color: AppColors.error.withValues(alpha: 0.25)),
      ),
      child: Row(
        children: [
          Icon(Icons.bug_report_outlined, color: AppColors.error.withValues(alpha: 0.9)),
          const SizedBox(width: 12),
          Expanded(
            child: Text(
              'Open bugs (all projects)',
              style: Theme.of(context).textTheme.bodyMedium?.copyWith(color: AppColors.textPrimary),
            ),
          ),
          Text(
            '$count',
            style: Theme.of(context).textTheme.titleLarge?.copyWith(
                  fontWeight: FontWeight.bold,
                  color: AppColors.error,
                ),
          ),
        ],
      ),
    );
  }
}

class _WorkloadBar extends StatelessWidget {
  final String label;
  final double flexInProgress;
  final double flexPending;
  final double flexOnHold;

  const _WorkloadBar({
    required this.label,
    required this.flexInProgress,
    required this.flexPending,
    required this.flexOnHold,
  });

  @override
  Widget build(BuildContext context) {
    return Container(
      padding: const EdgeInsets.all(18),
      decoration: BoxDecoration(
        color: AppColors.cardBg,
        borderRadius: BorderRadius.circular(16),
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Text(
            label,
            style: Theme.of(context).textTheme.titleSmall?.copyWith(
                  fontWeight: FontWeight.w600,
                  color: AppColors.textPrimary,
                ),
          ),
          const SizedBox(height: 12),
          ClipRRect(
            borderRadius: BorderRadius.circular(6),
            child: Row(
              children: [
                Expanded(
                  flex: (flexInProgress * 100).round().clamp(1, 1000),
                  child: Container(height: 10, color: AppColors.accent),
                ),
                Expanded(
                  flex: (flexPending * 100).round().clamp(1, 1000),
                  child: Container(height: 10, color: AppColors.accentDark),
                ),
                Expanded(
                  flex: (flexOnHold * 100).round().clamp(1, 1000),
                  child: Container(height: 10, color: AppColors.textMuted),
                ),
              ],
            ),
          ),
          const SizedBox(height: 8),
          Wrap(
            spacing: 12,
            runSpacing: 4,
            children: [
              _legend(context, AppColors.accent, 'In progress'),
              _legend(context, AppColors.accentDark, 'Pending'),
              _legend(context, AppColors.textMuted, 'On hold'),
            ],
          ),
        ],
      ),
    );
  }

  Widget _legend(BuildContext context, Color c, String text) {
    return Row(
      mainAxisSize: MainAxisSize.min,
      children: [
        Container(
          width: 10,
          height: 10,
          decoration: BoxDecoration(color: c, borderRadius: BorderRadius.circular(2)),
        ),
        const SizedBox(width: 6),
        Text(text, style: Theme.of(context).textTheme.bodySmall?.copyWith(color: AppColors.textSecondary)),
      ],
    );
  }
}

class _ActiveProjectsCard extends StatelessWidget {
  final int activeCount;

  const _ActiveProjectsCard({required this.activeCount});

  @override
  Widget build(BuildContext context) {
    return Container(
      padding: const EdgeInsets.all(18),
      decoration: BoxDecoration(
        color: AppColors.cardBg,
        borderRadius: BorderRadius.circular(16),
      ),
      child: Row(
        children: [
          Expanded(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(
                  'Active projects',
                  style: Theme.of(context).textTheme.titleMedium?.copyWith(
                        fontWeight: FontWeight.w600,
                        color: AppColors.textPrimary,
                      ),
                ),
                const SizedBox(height: 4),
                Text(
                  'Status = Active',
                  style: Theme.of(context).textTheme.bodySmall?.copyWith(color: AppColors.textMuted),
                ),
              ],
            ),
          ),
          Text(
            '$activeCount',
            style: Theme.of(context).textTheme.headlineMedium?.copyWith(
                  fontWeight: FontWeight.bold,
                  color: AppColors.accent,
                ),
          ),
        ],
      ),
    );
  }
}

class _AdminOrgCard extends StatelessWidget {
  final int users;
  final int teams;

  const _AdminOrgCard({required this.users, required this.teams});

  @override
  Widget build(BuildContext context) {
    return Container(
      padding: const EdgeInsets.all(18),
      decoration: BoxDecoration(
        color: AppColors.cardBg,
        borderRadius: BorderRadius.circular(16),
        border: Border.all(color: AppColors.accent.withValues(alpha: 0.2)),
      ),
      child: Row(
        children: [
          Expanded(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(
                  'Organization',
                  style: Theme.of(context).textTheme.titleMedium?.copyWith(
                        fontWeight: FontWeight.w600,
                        color: AppColors.textPrimary,
                      ),
                ),
                const SizedBox(height: 8),
                Text('$users users · $teams teams', style: const TextStyle(color: AppColors.textSecondary)),
              ],
            ),
          ),
          const Icon(Icons.admin_panel_settings_outlined, color: AppColors.accent, size: 32),
        ],
      ),
    );
  }
}

class _ProjectsSection extends StatelessWidget {
  final List<ProjectModel> projects;
  final String title;
  final String subtitle;

  const _ProjectsSection({
    required this.projects,
    required this.title,
    required this.subtitle,
  });

  @override
  Widget build(BuildContext context) {
    final provider = context.watch<ProjectProvider>();

    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Row(
          mainAxisAlignment: MainAxisAlignment.spaceBetween,
          children: [
            Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(
                  title,
                  style: Theme.of(context).textTheme.titleMedium?.copyWith(
                        fontWeight: FontWeight.bold,
                        color: AppColors.textPrimary,
                      ),
                ),
                Text(
                  subtitle,
                  style: Theme.of(context).textTheme.bodySmall?.copyWith(color: AppColors.textMuted),
                ),
              ],
            ),
          ],
        ),
        const SizedBox(height: 12),
        if (provider.isLoadingProjects)
          const Center(child: Padding(padding: EdgeInsets.all(24), child: CircularProgressIndicator(color: AppColors.accent)))
        else if (projects.isEmpty)
          const Center(child: Text('No projects found', style: TextStyle(color: AppColors.textMuted)))
        else
          ...projects.map((p) => _ProjectTile(name: p.title, progress: p.completionPercentage)),
      ],
    );
  }
}

class _ProjectTile extends StatelessWidget {
  final String name;
  final double progress;

  const _ProjectTile({required this.name, required this.progress});

  @override
  Widget build(BuildContext context) {
    final p = progress.clamp(0.0, 1.0);
    return Container(
      margin: const EdgeInsets.only(bottom: 12),
      padding: const EdgeInsets.all(16),
      decoration: BoxDecoration(
        color: AppColors.cardBg,
        borderRadius: BorderRadius.circular(12),
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
                  borderRadius: BorderRadius.circular(8),
                ),
                child: const Icon(Icons.folder_rounded, color: AppColors.accent, size: 20),
              ),
              const SizedBox(width: 12),
              Expanded(
                child: Text(
                  name,
                  style: Theme.of(context).textTheme.titleSmall?.copyWith(
                        fontWeight: FontWeight.w600,
                        color: AppColors.textPrimary,
                      ),
                ),
              ),
              Text(
                '${(p * 100).round()}%',
                style: Theme.of(context).textTheme.bodySmall?.copyWith(
                      fontWeight: FontWeight.bold,
                      color: AppColors.accent,
                    ),
              ),
            ],
          ),
          const SizedBox(height: 10),
          ClipRRect(
            borderRadius: BorderRadius.circular(4),
            child: LinearProgressIndicator(
              value: p,
              minHeight: 6,
              backgroundColor: AppColors.cardBgLighter,
              valueColor: const AlwaysStoppedAnimation<Color>(AppColors.accent),
            ),
          ),
        ],
      ),
    );
  }
}
