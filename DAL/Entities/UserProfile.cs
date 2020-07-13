using System;
using System.Collections.Generic;

namespace DAL.Entities
{
    public class UserProfile
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool AllowEmailNotifications { get; set; }
        public Guid? ProjectId { get; set; }

        public virtual Project Project { get; set; }
        public virtual ICollection<Project> ManagedProjects { get; set; }
        public virtual ICollection<Assignment> IssuedAssignments { get; set; }
    }
}