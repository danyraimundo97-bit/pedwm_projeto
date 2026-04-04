using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationLayer.Commands
{
    public class AssignTaskToUserCommand
    {
        public string ProjectId { get; set; }
        public string TaskId { get; set; }
        public string AssigneeUserId { get; set; }
    }
}
