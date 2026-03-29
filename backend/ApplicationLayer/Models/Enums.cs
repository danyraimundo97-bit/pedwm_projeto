namespace ApplicationLayer.Models
{
    public enum ProjectType
    {
        Standard,
        SickLeave,
        Training,
        Holiday,
    }

    public enum TaskType
    {
        Feature,
        Bug,
    }

    public enum NotificationType
    {
        Info,
        Warning,
        Alert,
    }

    public enum HolidayType
    {
        Fixed,
        Optional,
    }

    public enum UserRole
    {
        Admin,
        GP,
        Standard,
    }
}
