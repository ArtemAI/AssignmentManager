using System;
using System.ComponentModel.DataAnnotations;

namespace BLL.Models
{
    public class RegisterUserDto
    {
        public Guid Id { get; set; }
        [Required]
        public string UserName { get; set; }
		[Required]
		public string Password { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
		public string Email { get; set; }
        [Required]
        public bool AllowEmailNotifications { get; set; }
    }
}