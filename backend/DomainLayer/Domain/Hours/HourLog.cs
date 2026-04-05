namespace DomainLayer.Domain.Hours
{
    /// <summary>One line of consumed hours on a project (optionally attributed to a task).</summary>
    public sealed class HourLog
    {
        public Guid Id { get; set; }

        public Guid ProjectId { get; set; }

        public Guid? TaskId { get; set; }

        public double Hours { get; set; }

        public DateTime LoggedAtUtc { get; set; }

        public Guid UserId { get; set; }
    }
}
