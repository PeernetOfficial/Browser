using System;
using Peernet.Browser.Application.Managers;

namespace Peernet.Browser.WPF.Services
{
    public class SettingsManager : ISettingsManager
    {
        private static readonly Guid apiKey = Guid.NewGuid();

        public string ApiKey => apiKey.ToString();

        public string ApiUrl
        {
            get => Get(nameof(ApiUrl));
            set => Set(nameof(ApiUrl), value);
        }

        public Uri SocketUrl => GetSocket();

        public string Backend
        {
            get => Get(nameof(Backend));
            set => Set(nameof(Backend), value);
        }

        public string DownloadPath
        {
            get => Environment.ExpandEnvironmentVariables(Get(nameof(DownloadPath)));
            set => Set(nameof(DownloadPath), value);
        }

        private static string Get(string key) => System.Configuration.ConfigurationManager.AppSettings[key];

        private static void Set(string key, string value) => System.Configuration.ConfigurationManager.AppSettings[key] = value;

        private Uri GetSocket()
        {
            var socketAddress = ApiUrl.StartsWith("https:") ? ApiUrl.Replace("https:", "wss:") : ApiUrl.Replace("http:", "ws:");
            return new Uri(new Uri(socketAddress), $"console?k={ApiKey}");
        }
    }
}