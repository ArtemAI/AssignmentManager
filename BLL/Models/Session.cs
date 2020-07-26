using System;

namespace BLL.Models
{
    public class Session
    {
        public ApplicationUser User { get; set; }

        public Guid UserId { get; set; }
    }
}
