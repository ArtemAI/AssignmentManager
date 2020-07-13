using System;
using System.ComponentModel.DataAnnotations;

namespace BLL.Models
{
    public class UserProfileDto
    {
        public Guid Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public bool AllowEmailNotifications { get; set; }
        public Guid ProjectId { get; set; }
    }
}
