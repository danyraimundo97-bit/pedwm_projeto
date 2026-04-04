using System;

namespace PresentationLayer.DTOs
{
    public class ProjectResponse_DTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string Status { get; set; } = string.Empty;
        public int ConsumedHours { get; set; }
    }
}