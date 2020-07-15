using BLL.Models;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IEmailService
    {
        Task Send(EmailMessage emailMessage);
    }
}