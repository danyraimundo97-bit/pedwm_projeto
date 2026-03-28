import '../../models/team_model.dart';

const _kMockNetworkDelay = Duration(milliseconds: 1000);

Future<List<TeamModel>> fetchTeamsFromBackend() async {
  await Future<void>.delayed(_kMockNetworkDelay);
  return [
    TeamModel(id: 'team1', name: 'Platform Squad', memberUserIds: ['u1', 'u2']),
  ];
}

Future<void> createTeamInBackend(String name) async {
  await Future<void>.delayed(_kMockNetworkDelay);
}

Future<void> addUserToTeamInBackend(String teamId, String userId) async {
  await Future<void>.delayed(_kMockNetworkDelay);
}