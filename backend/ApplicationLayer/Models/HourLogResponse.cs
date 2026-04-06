namespace ApplicationLayer.Models
{
    public sealed class HourLogResponse
    {
        public Guid Id { get; init; }

        public Guid ProjectId { get; init; }

        public string ProjectTitle { get; init; } = string.Empty;

        public Guid? TaskId { get; init; }

        public string? TaskTitle { get; init; }

        public double Hours { get; init; }

        public DateTime LoggedAtUtc { get; init; }
    }
}
