using System;
using System.Configuration;
using Peernet.Browser.Application.Managers;
using Peernet.Browser.Models.Presentation;

namespace Peernet.Browser.WPF.Services
{
    public class SettingsManager : ISettingsManager
    {
        private static readonly Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

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

        public VisualMode DefaultTheme
        {
            get => (VisualMode)Enum.Parse(typeof(VisualMode), Get(nameof(DefaultTheme)) ?? default(VisualMode).ToString());
            set => Set(nameof(DefaultTheme), value.ToString());
        }

        private static string Get(string key) => config.AppSettings.Settings[key]?.Value;

        private static void Set(string key, string value)
        {
            var setting = config.AppSettings.Settings[key];
            if (value != null)
            {
                if (setting == null)
                {
                    config.AppSettings.Settings.Add(key, value);
                }
                else
                {
                    setting.Value = value;
                }
            }
            else
            {
                config.AppSettings.Settings.Remove(key);
            }
        }

        private Uri GetSocket()
        {
            var socketAddress = ApiUrl.StartsWith("https:") ? ApiUrl.Replace("https:", "wss:") : ApiUrl.Replace("http:", "ws:");
            return new Uri(new Uri(socketAddress), $"console?k={ApiKey}");
        }

        public void Save()
        {
            config.Save();
        }
    }
}