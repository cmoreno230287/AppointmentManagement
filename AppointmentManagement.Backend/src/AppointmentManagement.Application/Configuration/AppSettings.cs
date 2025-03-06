using Microsoft.Extensions.Configuration;

namespace AppointmentManagement.Application.Configuration
{
    public class AppSettings
    {
        public JwtSettings JwtSettings { get; set; } = new JwtSettings();
        public DatabaseSettings DatabaseSettings { get; set; } = new DatabaseSettings();
        public LoggingSettings LoggingSettings { get; set; } = new LoggingSettings();
        public EncryptionSettings EncryptionSettings { get; set; } = new EncryptionSettings();
    }

    public class JwtSettings
    {
        public string Secret { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int ExpirationMinutes { get; set; }
    }

    public class DatabaseSettings
    {
        public string ConnectionString { get; set; } = string.Empty;
    }

    public class LoggingSettings
    {
        public string MinimumLevel { get; set; } = "Information";
    }

    public class EncryptionSettings
    {
        public string EncryptionKey { get; set; } 
    }
}
