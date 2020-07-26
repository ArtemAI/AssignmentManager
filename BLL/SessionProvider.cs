using BLL.Models;

namespace BLL
{
    /// <summary>
    /// Provides information about user of certain session.
    /// </summary>
    public class SessionProvider
    {
        public Session Session;

        public SessionProvider()
        {
            Session = new Session();
        }

        public void Initialize(ApplicationUser user)
        {
            Session.User = user;
            Session.UserId = user.Id;
        }
    }
}
