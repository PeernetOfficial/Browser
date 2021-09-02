namespace Peernet.Browser.Application.Services
{
    public interface ISettingsManager
    {
        string ApiUrl { get; set; }
        string SocketUrl { get; set; }
    }
}