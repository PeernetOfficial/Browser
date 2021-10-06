using Peernet.Browser.Application.Managers;

namespace Peernet.Browser.WPF
{
    public class SettingsManager : ISettingsManager
    {
        public string ApiUrl
        {
            get => Properties.Settings.Default.ApiUrl;
            set
            {
                Properties.Settings.Default.ApiUrl = value;
                Properties.Settings.Default.Save();
            }
        }

        public string SocketUrl
        {
            get => Properties.Settings.Default.SocketUrl;
            set
            {
                Properties.Settings.Default.SocketUrl = value;
                Properties.Settings.Default.Save();
            }
        }

        public string CmdPath
        {
            get => Properties.Settings.Default.CmdPath;
            set
            {
                Properties.Settings.Default.CmdPath = value;
                Properties.Settings.Default.Save();
            }
        }
    }
}