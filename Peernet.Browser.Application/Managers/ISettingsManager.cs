namespace Peernet.Browser.Application.Managers
{
    public interface ISettingsManager
    {
        string ApiKey { get; }

        string ApiUrl { get; set; }

        string SocketUrl { get; set; }

        string Backend { get; set; }

        string DownloadPath { get; set; }
    }
}