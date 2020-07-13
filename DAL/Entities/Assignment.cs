using System;

namespace DAL.Entities
{
    public enum AssignmentStatus
    {
        ToDo,
        InProgress,
        Done
    }

    public class Assignment
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public int CompletionPercent { get; set; }
        public AssignmentStatus Status { get; set; }
        public DateTime Deadline { get; set; }
        public Guid ProjectId { get; set; }
        public Guid AssigneeId { get; set; }

        public virtual Project Project { get; set; }
        public virtual UserProfile Assignee { get; set; }
    }
}
