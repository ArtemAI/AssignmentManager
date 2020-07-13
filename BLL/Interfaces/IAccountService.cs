using System;
using BLL.Models;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IAccountService : IDisposable
    {
        Task<ApplicationUser> Login(string email, string password);
        Task<ApplicationUser> Register(RegisterUserDto user);
        Task<object> GenerateJwt(ApplicationUser user);
    }
}
