import '../../models/hours_log_entry.dart';
import '../../models/task_model.dart';

const _kMockNetworkDelay = Duration(milliseconds: 1000);
 
 Future<List<TaskHoursInMonth>> fetchTaskHoursBreakdownForMonthFromBackend(DateTime month) async {
    await Future<void>.delayed(_kMockNetworkDelay);
    return [
      TaskHoursInMonth(projectTitle: 'Project 1', taskTitle: 'Task 1', hours: 10),
      TaskHoursInMonth(projectTitle: 'Project 2', taskTitle: 'Task 2', hours: 20),
      TaskHoursInMonth(projectTitle: 'Project 3', taskTitle: 'Task 3', hours: 30),
    ];
  }

   Future<void> updateTaskStatusInBackend(String projectId, String taskId, TaskStatus newStatus) async {
    await Future<void>.delayed(_kMockNetworkDelay);
  }

  Future<void> updateTaskAssigneeInBackend(String projectId, String taskId, String? assigneeUserId) async {
    await Future<void>.delayed(_kMockNetworkDelay);
  }

  Future<void> createTaskInBackend(String projectId, String title, String description, int estimate, TaskType type, String? severity, String? assigneeUserId) async {
    await Future<void>.delayed(_kMockNetworkDelay);
  }