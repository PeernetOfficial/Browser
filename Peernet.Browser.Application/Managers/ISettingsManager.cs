﻿namespace Peernet.Browser.Application.Managers
{
    public interface ISettingsManager
    {
        string ApiUrl { get; set; }

        string SocketUrl { get; set; }

        string CmdPath { get; set; }

        string DownloadPath { get; set; }
    }
}