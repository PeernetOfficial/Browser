namespace Peernet.Browser.Models.Domain.Blockchain
{
    public class ApiBlockchainHeader
    {
        public string PeerId { get; set; }

        public int Version { get; set; }

        public int Height { get; set; }
    }
}