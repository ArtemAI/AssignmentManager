using System;
using System.Collections.Generic;

namespace DAL.Entities
{
    public class Project
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? ManagerId { get; set; }

        public virtual UserProfile Manager { get; set; }
        public virtual ICollection<Assignment> Assignments { get; set; }
        public virtual ICollection<UserProfile> Users { get; set; }
    }
}