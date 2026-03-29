import '../../models/hours_log_entry.dart';
import '../../models/project_model.dart';
import '../../models/task_model.dart';

const _kMockNetworkDelay = Duration(milliseconds: 1000);

  Future<List<MonthlyHoursStats>> fetchMonthlyHoursStatsFromBackend() async {
    await Future<void>.delayed(_kMockNetworkDelay);
    return [
      MonthlyHoursStats(
        month: DateTime.now(),
        projectCount: 2,
        taskCount: 4,
        totalHours: 20,
      ),
      MonthlyHoursStats(
        month: DateTime.now().subtract(const Duration(days: 30)),
        projectCount: 6,
        taskCount: 14,
        totalHours: 40,
      ),
      MonthlyHoursStats(
        month: DateTime.now().subtract(const Duration(days: 60)),
        projectCount: 6,
        taskCount: 10,
        totalHours: 40,
      ),
    ];
  }
  
  
  Future<List<ProjectModel>> fetchProjectsFromBackend() async {
      await Future<void>.delayed(_kMockNetworkDelay);
      var projects = [
        ProjectModel(
          id: '1',
          title: 'Web Platform',
          type: ProjectType.standard,
          description: 'GraphQL Integration',
          status: ProjectStatus.fromString('Active'),
          budgetHours: 50,
          consumedHours: 0,
          completionPercentage: 0,
          tasks: [
            TaskModel(
              id: 't2',
              title: 'Fix Login Crash',
              description:
                  'App crashes on login when the session cookie is expired. Reproduce: cold start, tap Login with saved credentials.',
              status: TaskStatus.fromString('ToDo'),
              type: TaskType.fromString('Bug'),
              estimate: 3,
              loggedHours: 20,
              severity: 'Critical',
              assigneeUserId: null,
            ),
            TaskModel(
              id: 't5',
              title: 'Typo in Settings',
              description: "Settings > About shows 'Ver': fix label to 'Version'.",
              status: TaskStatus.fromString('Active'),
              type: TaskType.fromString('Bug'),
              estimate: 1,
              loggedHours: 12,
              severity: 'Low',
              assigneeUserId: 'u1',
            ),
          ],
        ),
        ProjectModel(
          id: '2',
          title: 'Mobile App v2',
          type: ProjectType.training,
          description: 'Native features',
          status: ProjectStatus.fromString('Active'),
          budgetHours: 40,
          consumedHours: 0,
          completionPercentage: 0,
          tasks: [
            TaskModel(
              id: 't3',
              title: 'Update Flutter SDK',
              description:
                  'Bump to stable 3.x, run flutter pub upgrade, fix deprecations in android/ios configs.',
              status: TaskStatus.fromString('Completed'),
              type: TaskType.fromString('Feature'),
              estimate: 5,
              loggedHours: 10,
              assigneeUserId: 'u2',
            ),
            TaskModel(
              id: 't4',
              title: 'Profile Image Upload',
              description:
                  'Allow picking from gallery or camera, crop to square, upload via GraphQL mutation with progress.',
              status: TaskStatus.fromString('Active'),
              type: TaskType.fromString('Feature'),
              estimate: 13,
              loggedHours: 8,
              assigneeUserId: 'u2',
            ),
          ],
        ),
      ];
      return projects;
  }

  Future<void> createProjectInBackend(
    String title,
    String description,
    int budgetHours,
    ProjectType type,
  ) async {
    await Future<void>.delayed(_kMockNetworkDelay);
  }