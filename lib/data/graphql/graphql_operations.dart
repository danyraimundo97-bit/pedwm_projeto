import 'package:graphql/client.dart';

/// Central GraphQL documents aligned with Hot Chocolate schema (camelCase fields).
class GraphqlOperations {
  GraphqlOperations._();

  static final projectsAndTasksQuery = gql(r'''
    query ProjectsAndTasks {
      projects {
        id
        title
        startDate
        endDate
        type
      }
      tasks {
        id
        title
        description
        status
        projectId
        assignedUserId
        taskType
        environment
        severity
        storyPoints
      }
    }
  ''');

  static final createProjectMutation = gql(r'''
    mutation CreateProject($input: CreateProject_DTOInput!) {
      createProject(input: $input)
    }
  ''');

  static final createTaskMutation = gql(r'''
    mutation CreateTask($input: CreateTask_DTOInput!) {
      createTask(input: $input)
    }
  ''');

  static final createUserMutation = gql(r'''
    mutation CreateUser($input: CreateUser_DTOInput!) {
      createUser(input: $input)
    }
  ''');

  static final createTeamMutation = gql(r'''
    mutation CreateTeam($input: CreateTeam_DTOInput!) {
      createTeam(input: $input)
    }
  ''');

  static final assignTaskToUserMutation = gql(r'''
    mutation AssignTaskToUser($input: AssignUserToTask_DTOInput!) {
      assignTaskToUser(input: $input)
    }
  ''');

  static final assignUserToTeamMutation = gql(r'''
    mutation AssignUserToTeam($input: AssignUserToTeam_DTOInput!) {
      assignUserToTeam(input: $input)
    }
  ''');

  static final addHoursToProjectMutation = gql(r'''
    mutation AddHoursToProject($input: AddHoursToProject_DTOInput!) {
      addHoursToProject(input: $input)
    }
  ''');

  static final changeProjectStatusMutation = gql(r'''
    mutation ChangeProjectStatus($input: ChangeProjectStatus_DTOInput!) {
      changeProjectStatus(input: $input)
    }
  ''');
}
