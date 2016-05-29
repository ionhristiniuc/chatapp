using System.Configuration;

namespace DataServiceClient.Configuration
{
    public static class ConfigManager
    {
        public static readonly string ServiceUrl = ConfigurationManager.AppSettings["BaseUrl"];
        public static readonly string AuthenticationPath = ConfigurationManager.AppSettings["AuthPath"];
    }
}