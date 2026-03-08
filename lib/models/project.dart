class Project {
  final String id;
  final String name;
  final String description;

  Project({required this.id, required this.name, required this.description});

  factory Project.fromJson(Map<String, dynamic> json) {
    return Project(
      id: json['id'],
      name: json['name'],
      description: json['description'],
    );
  }
}