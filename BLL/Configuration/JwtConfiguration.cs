using BLL.Interfaces;

namespace BLL.Configuration
{
    public class JwtConfiguration : IJwtConfiguration
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public double ExpirationDays { get; set; }
    }
}