using System;

namespace Peernet.Browser.Application.Managers
{
    public interface ISettingsManager
    {
        string ApiKey { get; }

        string ApiUrl { get; set; }

        Uri SocketUrl { get; }

        string Backend { get; set; }

        string DownloadPath { get; set; }

        void Save();
    }
}