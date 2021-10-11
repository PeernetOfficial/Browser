namespace Peernet.Browser.Models.Domain.Common
{
    public class ApiResponseStatus
    {
        public int Status { get; set; }

        public bool IsConnected { get; set; }

        public int CountPeerList { get; set; }

        public int CountNetwork { get; set; }
    }
}