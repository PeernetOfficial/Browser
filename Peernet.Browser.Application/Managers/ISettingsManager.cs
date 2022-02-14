using Peernet.Browser.Models.Presentation;
using System;

namespace Peernet.Browser.Application.Managers
{
    public interface ISettingsManager
    {
        string ApiKey { get; }

        string ApiUrl { get; set; }

        Uri SocketUrl { get; }

        string Backend { get; set; }

        string LogFile { get; }

        string DownloadPath { get; set; }

        VisualMode DefaultTheme { get; set; }

        void Save();
    }
}