namespace Peernet.Browser.Application.Models
{
    public class Status
    {
        public int StatusCode { get; set; }

        public bool IsConnected { get; set; }

        public ulong CountPeerList { get; set; }
    }
}