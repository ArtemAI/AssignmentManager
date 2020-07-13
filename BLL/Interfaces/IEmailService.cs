using BLL.Models;

namespace BLL.Interfaces
{
    public interface IEmailService
    {
        void Send(EmailMessage emailMessage);
    }
}