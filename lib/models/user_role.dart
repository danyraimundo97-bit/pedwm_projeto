enum UserRole {
  admin,
  projectManager,
  member,
}

extension UserRoleLabel on UserRole {
  String get label {
    switch (this) {
      case UserRole.admin:
        return 'Admin';
      case UserRole.projectManager:
        return 'Project Manager';
      case UserRole.member:
        return 'Member';
    }
  }
}
