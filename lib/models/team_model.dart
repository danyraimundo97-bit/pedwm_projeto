class TeamModel {
  final String id;
  final String name;
  final List<String> memberUserIds;

  TeamModel({
    required this.id,
    required this.name,
    this.memberUserIds = const [],
  });
}
