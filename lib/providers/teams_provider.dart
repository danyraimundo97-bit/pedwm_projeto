import 'package:flutter/foundation.dart';
import 'package:graphql/client.dart';
import '../models/team_model.dart';
import '../data/repositories/team_repository.dart' as team_repo;

/// Teams and membership (CRUD + backend sync).
class TeamsProvider extends ChangeNotifier {
  TeamsProvider(this._client) {
    fetchTeams();
  }

  final GraphQLClient _client;

  List<TeamModel> _teams = [];

  List<TeamModel> get teams => List.unmodifiable(_teams);

  Future<void> fetchTeams() async {
    _teams = await team_repo.fetchTeamsFromBackend();
    notifyListeners();
  }

  Future<void> createTeam(String name) async {
    final id = 'team${DateTime.now().millisecondsSinceEpoch}';
    _teams.add(TeamModel(id: id, name: name));
    notifyListeners();
    try {
      await team_repo.createTeamInBackend(_client, name: name);
      await fetchTeams();
    } catch (e, st) {
      debugPrint('createTeamInBackend: $e\n$st');
      rethrow;
    }
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
    notifyListeners();
    try {
      await team_repo.addUserToTeamInBackend(_client, teamId: teamId, userId: userId);
      await fetchTeams();
    } catch (e, st) {
      debugPrint('addUserToTeamInBackend: $e\n$st');
      rethrow;
    }
  }
}
