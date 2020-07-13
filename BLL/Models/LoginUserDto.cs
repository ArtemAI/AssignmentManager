using System.ComponentModel.DataAnnotations;

namespace BLL.Models
{
    public class LoginUserDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
