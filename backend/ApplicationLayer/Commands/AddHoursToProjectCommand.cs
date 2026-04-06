namespace ApplicationLayer.Commands
{
    public class AddHoursToProjectCommand
    {
        public string ProjectId { get; set; } = string.Empty;

        public double Hours { get; set; }

        /// <summary>Optional task to attribute hours to (must belong to the project).</summary>
        public string? TaskId { get; set; }

        /// <summary>Set by the handler from the current session (hour log audit).</summary>
        public Guid UserId { get; set; }
    }
}
