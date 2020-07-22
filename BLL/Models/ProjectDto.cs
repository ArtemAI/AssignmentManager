using System;
using System.ComponentModel.DataAnnotations;

namespace BLL.Models
{
    public class ProjectDto
    {
        public Guid? Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid ManagerId { get; set; }
    }
}
