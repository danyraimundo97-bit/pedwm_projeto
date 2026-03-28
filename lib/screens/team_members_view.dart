import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import '../models/app_user.dart';
import '../models/team_model.dart';
import '../providers/auth_provider.dart';
import '../theme/app_colors.dart';

/// Manage members per team (PM + Admin).
class TeamMembersView extends StatelessWidget {
  const TeamMembersView({super.key});

  @override
  Widget build(BuildContext context) {
    final auth = context.watch<AuthProvider>();

    return Scaffold(
      backgroundColor: AppColors.background,
      appBar: AppBar(
        backgroundColor: AppColors.background,
        iconTheme: const IconThemeData(color: AppColors.textPrimary),
        title: const Text('Teams & members', style: TextStyle(color: AppColors.textPrimary, fontWeight: FontWeight.bold)),
      ),
      body: ListView.builder(
        padding: const EdgeInsets.all(20),
        itemCount: auth.teams.length,
        itemBuilder: (context, index) {
          final team = auth.teams[index];
          return Padding(
            padding: const EdgeInsets.only(bottom: 12),
            child: _TeamCard(team: team, auth: auth),
          );
        },
      ),
    );
  }
}

class _TeamCard extends StatelessWidget {
  final TeamModel team;
  final AuthProvider auth;

  const _TeamCard({required this.team, required this.auth});

  @override
  Widget build(BuildContext context) {
    final members = <AppUser>[];
    for (final id in team.memberUserIds) {
      for (final u in auth.users) {
        if (u.id == id) {
          members.add(u);
          break;
        }
      }
    }

    return Container(
      padding: const EdgeInsets.all(16),
      decoration: BoxDecoration(
        color: AppColors.cardBg,
        borderRadius: BorderRadius.circular(12),
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Text(team.name, style: const TextStyle(color: AppColors.textPrimary, fontWeight: FontWeight.bold, fontSize: 16)),
          const SizedBox(height: 8),
          ...members.map(
            (u) => Padding(
              padding: const EdgeInsets.only(bottom: 4),
              child: Text('${u.name} · ${u.email}', style: const TextStyle(color: AppColors.textSecondary, fontSize: 13)),
            ),
          ),
          if (auth.canAddTeamMembers) ...[
            const SizedBox(height: 12),
            OutlinedButton.icon(
              onPressed: () => _pickUser(context),
              icon: const Icon(Icons.person_add, size: 18),
              label: const Text('Add member'),
            ),
          ],
        ],
      ),
    );
  }

  Future<void> _pickUser(BuildContext context) async {
    final candidates = auth.users.where((u) => !team.memberUserIds.contains(u.id)).toList();
    if (candidates.isEmpty) {
      ScaffoldMessenger.of(context).showSnackBar(const SnackBar(content: Text('No users to add')));
      return;
    }
    final picked = await showModalBottomSheet<String>(
      context: context,
      backgroundColor: AppColors.cardBg,
      builder: (ctx) => SafeArea(
        child: ListView(
          children: candidates
              .map(
                (u) => ListTile(
                  title: Text(u.name, style: const TextStyle(color: AppColors.textPrimary)),
                  subtitle: Text(u.email, style: const TextStyle(color: AppColors.textMuted)),
                  onTap: () => Navigator.pop(ctx, u.id),
                ),
              )
              .toList(),
        ),
      ),
    );
    if (picked != null && context.mounted) {
      context.read<AuthProvider>().addUserToTeam(teamId: team.id, userId: picked);
    }
  }
}
