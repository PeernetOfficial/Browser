using Peernet.Browser.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
