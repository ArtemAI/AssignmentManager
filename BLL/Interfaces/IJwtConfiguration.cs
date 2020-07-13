namespace BLL.Interfaces
{
    public interface IJwtConfiguration
    {
        string SecretKey { get; }
        string Issuer { get; }
        string Audience { get; }
        double ExpirationDays { get; }
    }
}
