using ApplicationLayer.Commands;
using Mapster;
using PresentationLayer.DTOs;

namespace PresentationLayer.GraphQL.Mapping
{
    /// <summary>
    /// Adapter edge: GraphQL/API input models → application commands. Mapster is used only here in Presentation.
    /// </summary>
    public static class GraphQlInputToCommandMapper
    {
        public static CreateProjectCommand ToCommand(this CreateProject_DTO input) =>
            input.Adapt<CreateProjectCommand>();

        public static CreateTaskCommand ToCommand(this CreateTask_DTO input) =>
            input.Adapt<CreateTaskCommand>();

        public static CreateUserCommand ToCommand(this CreateUser_DTO input) =>
            input.Adapt<CreateUserCommand>();

        public static CreateTeamCommand ToCommand(this CreateTeam_DTO input) =>
            input.Adapt<CreateTeamCommand>();
        public static AssignTaskToUserCommand ToCommand(this AssignUserToTask_DTO input) =>
            input.Adapt<AssignTaskToUserCommand>();

    }
}
