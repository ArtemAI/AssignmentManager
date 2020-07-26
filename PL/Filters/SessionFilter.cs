using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using BLL;
using BLL.Models;

namespace PL.Filters
{
    /// <summary>
    /// Initializes SessionProvider instance.
    /// </summary>
    public class SessionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var serviceProvider = context.HttpContext.RequestServices;
            var sessionProvider = serviceProvider.GetService(typeof(SessionProvider)) as SessionProvider;
            var userManager = serviceProvider.GetService(typeof(UserManager<ApplicationUser>)) as UserManager<ApplicationUser>;

            var user = await userManager.GetUserAsync(context.HttpContext.User);
            if (user != null)
            {
                sessionProvider.Initialize(user);
            }

            await next();
        }
    }
}