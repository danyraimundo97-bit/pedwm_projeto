namespace PresentationLayer.DTOs
{
    public class AddHoursToProject_DTO
    {
        public string ProjectId { get; set; } = string.Empty;

        public double Hours { get; set; }

        public string? TaskId { get; set; }
    }
}
