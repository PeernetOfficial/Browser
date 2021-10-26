using Peernet.Browser.Application.Managers;

namespace Peernet.Browser.WPF.Services
{
    public class SettingsManager : ISettingsManager
    {
        public string ApiUrl
        {
            get => Get(nameof(ApiUrl));
            set => Set(nameof(ApiUrl), value);
        }

        public string SocketUrl
        {
            get => Get(nameof(SocketUrl));
            set => Set(nameof(SocketUrl), value);
        }

        public string CmdPath
        {
            get => Get(nameof(CmdPath));
            set => Set(nameof(CmdPath), value);
        }

        public string DownloadPath
        {
            get => Get(nameof(DownloadPath));
            set => Set(nameof(DownloadPath), value);
        }

        private static string Get(string key) => System.Configuration.ConfigurationManager.AppSettings[key];

        private static void Set(string key, string value) => System.Configuration.ConfigurationManager.AppSettings[key] = value;
    }
}