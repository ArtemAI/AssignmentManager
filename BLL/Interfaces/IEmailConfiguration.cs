namespace BLL.Interfaces
{
	public interface IEmailConfiguration
    {
        string SmtpServer { get; }
        int SmtpPort { get; }
        string SmtpUsername { get; }
        string SmtpPassword { get; }
        string SenderName { get; }
    }
}