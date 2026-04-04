using System;

namespace PresentationLayer.DTOs
{
    public class TaskResponse_DTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // Relações (Foreign Keys)
        public Guid ProjectId { get; set; }
        public Guid? AssigneeUserId { get; set; }
    }
}