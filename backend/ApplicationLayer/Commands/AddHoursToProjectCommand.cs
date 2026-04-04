namespace ApplicationLayer.Commands
{
    public class AddHoursToProjectCommand
    {
        public string ProjectId { get; set; } = string.Empty;

        public double Hours { get; set; }
    }
}
