class DbSchema {
  static const int version = 1;

  static const List<String> createStatements = [
    '''
CREATE TABLE teams (
  id TEXT PRIMARY KEY,
  name TEXT NOT NULL,
  description TEXT,
  department TEXT
)
''',
    '''
CREATE TABLE users (
  id TEXT PRIMARY KEY,
  name TEXT NOT NULL,
  email TEXT NOT NULL UNIQUE,
  team_id TEXT,
  role TEXT NOT NULL CHECK (role IN ('admin', 'gp', 'standard')),
  FOREIGN KEY (team_id) REFERENCES teams(id) ON DELETE SET NULL
)
''',
    '''
CREATE TABLE project_base (
  id TEXT PRIMARY KEY,
  title TEXT NOT NULL,
  type TEXT NOT NULL,
  hours INTEGER NOT NULL DEFAULT 0 CHECK (hours >= 0),
  kind TEXT NOT NULL CHECK (kind IN ('project', 'holiday', 'training'))
)
''',
    '''
CREATE TABLE projects (
  id TEXT PRIMARY KEY,
  budget_hours INTEGER NOT NULL DEFAULT 0 CHECK (budget_hours >= 0),
  client_name TEXT NOT NULL,
  status TEXT NOT NULL CHECK (status IN ('active', 'completed', 'on_hold')),
  manager_id TEXT,
  team_id TEXT,
  FOREIGN KEY (id) REFERENCES project_base(id) ON DELETE CASCADE,
  FOREIGN KEY (manager_id) REFERENCES users(id) ON DELETE SET NULL,
  FOREIGN KEY (team_id) REFERENCES teams(id) ON DELETE SET NULL
)
''',
    '''
CREATE TABLE holidays (
  id TEXT PRIMARY KEY,
  holiday_type TEXT NOT NULL CHECK (holiday_type IN ('fixed', 'optional')),
  FOREIGN KEY (id) REFERENCES project_base(id) ON DELETE CASCADE
)
''',
    '''
CREATE TABLE trainings (
  id TEXT PRIMARY KEY,
  course_name TEXT NOT NULL,
  FOREIGN KEY (id) REFERENCES project_base(id) ON DELETE CASCADE
)
''',
    '''
CREATE TABLE time_entries (
  id TEXT PRIMARY KEY,
  project_id TEXT NOT NULL,
  user_id TEXT NOT NULL,
  hours INTEGER NOT NULL CHECK (hours > 0),
  description TEXT,
  timestamp TEXT NOT NULL,
  FOREIGN KEY (project_id) REFERENCES project_base(id) ON DELETE CASCADE,
  FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE CASCADE
)
''',
    '''
CREATE TABLE presence_events (
  user_id TEXT NOT NULL,
  project_id TEXT NOT NULL,
  action TEXT NOT NULL CHECK (action IN ('editing', 'viewing')),
  timestamp TEXT NOT NULL,
  PRIMARY KEY (user_id, project_id, action, timestamp),
  FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE CASCADE,
  FOREIGN KEY (project_id) REFERENCES project_base(id) ON DELETE CASCADE
)
''',
    '''
CREATE TABLE task_base (
  id TEXT PRIMARY KEY,
  title TEXT NOT NULL,
  description TEXT,
  status TEXT NOT NULL CHECK (status IN ('active', 'completed', 'on_hold')),
  created_at TEXT NOT NULL,
  completed_at TEXT,
  assignee_id TEXT,
  project_id TEXT NOT NULL,
  task_type TEXT NOT NULL CHECK (task_type IN ('bug', 'feature')),
  FOREIGN KEY (assignee_id) REFERENCES users(id) ON DELETE SET NULL,
  FOREIGN KEY (project_id) REFERENCES project_base(id) ON DELETE CASCADE,
  CHECK (completed_at IS NULL OR completed_at >= created_at)
)
''',
    '''
CREATE TABLE bug_tasks (
  id TEXT PRIMARY KEY,
  environment TEXT NOT NULL,
  severity TEXT NOT NULL CHECK (severity IN ('high', 'mid', 'low')),
  FOREIGN KEY (id) REFERENCES task_base(id) ON DELETE CASCADE
)
''',
    '''
CREATE TABLE feature_tasks (
  id TEXT PRIMARY KEY,
  story_points INTEGER NOT NULL DEFAULT 0 CHECK (story_points >= 0),
  FOREIGN KEY (id) REFERENCES task_base(id) ON DELETE CASCADE
)
''',
    '''
CREATE TRIGGER trg_projects_kind_insert
BEFORE INSERT ON projects
FOR EACH ROW
BEGIN
  SELECT
    CASE
      WHEN (SELECT kind FROM project_base WHERE id = NEW.id) <> 'project'
      THEN RAISE(ABORT, 'project_base.kind must be project')
    END;
END
''',
    '''
CREATE TRIGGER trg_holidays_kind_insert
BEFORE INSERT ON holidays
FOR EACH ROW
BEGIN
  SELECT
    CASE
      WHEN (SELECT kind FROM project_base WHERE id = NEW.id) <> 'holiday'
      THEN RAISE(ABORT, 'project_base.kind must be holiday')
    END;
END
''',
    '''
CREATE TRIGGER trg_trainings_kind_insert
BEFORE INSERT ON trainings
FOR EACH ROW
BEGIN
  SELECT
    CASE
      WHEN (SELECT kind FROM project_base WHERE id = NEW.id) <> 'training'
      THEN RAISE(ABORT, 'project_base.kind must be training')
    END;
END
''',
    '''
CREATE TRIGGER trg_bug_tasks_type_insert
BEFORE INSERT ON bug_tasks
FOR EACH ROW
BEGIN
  SELECT
    CASE
      WHEN (SELECT task_type FROM task_base WHERE id = NEW.id) <> 'bug'
      THEN RAISE(ABORT, 'task_base.task_type must be bug')
    END;
END
''',
    '''
CREATE TRIGGER trg_feature_tasks_type_insert
BEFORE INSERT ON feature_tasks
FOR EACH ROW
BEGIN
  SELECT
    CASE
      WHEN (SELECT task_type FROM task_base WHERE id = NEW.id) <> 'feature'
      THEN RAISE(ABORT, 'task_base.task_type must be feature')
    END;
END
''',
    'CREATE INDEX idx_users_team_id ON users(team_id)',
    'CREATE INDEX idx_projects_manager_id ON projects(manager_id)',
    'CREATE INDEX idx_projects_team_id ON projects(team_id)',
    'CREATE INDEX idx_time_entries_project_id ON time_entries(project_id)',
    'CREATE INDEX idx_time_entries_user_id ON time_entries(user_id)',
    'CREATE INDEX idx_presence_project_id ON presence_events(project_id)',
    'CREATE INDEX idx_task_base_assignee_id ON task_base(assignee_id)',
    'CREATE INDEX idx_task_base_project_id ON task_base(project_id)',
    'CREATE INDEX idx_task_base_status ON task_base(status)',
  ];
}
