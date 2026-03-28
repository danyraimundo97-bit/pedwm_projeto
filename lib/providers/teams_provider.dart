import 'package:flutter/foundation.dart';
import '../models/team_model.dart';
import '../data/repositories/team_repository.dart' as team_repo;

/// Teams and membership (CRUD + backend sync).
class TeamsProvider extends ChangeNotifier {
  List<TeamModel> _teams = [];

  List<TeamModel> get teams => List.unmodifiable(_teams);

  TeamsProvider() {
    fetchTeams();
  }

  Future<void> fetchTeams() async {
    _teams = await team_repo.fetchTeamsFromBackend();
    notifyListeners();
  }

  Future<void> createTeam(String name) async {
    final id = 'team${DateTime.now().millisecondsSinceEpoch}';
    _teams.add(TeamModel(id: id, name: name));
    await team_repo.createTeamInBackend(name);
    notifyListeners();
  }

  Future<void> addUserToTeam({required String teamId, required String userId}) async {
    final i = _teams.indexWhere((t) => t.id == teamId);
    if (i == -1) return;
    final t = _teams[i];
    if (t.memberUserIds.contains(userId)) return;
    _teams[i] = TeamModel(
      id: t.id,
      name: t.name,
      memberUserIds: [...t.memberUserIds, userId],
    );
    await team_repo.addUserToTeamInBackend(teamId, userId);
    notifyListeners();
  }
}
