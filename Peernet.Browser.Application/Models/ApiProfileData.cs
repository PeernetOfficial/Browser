using System.Collections.Generic;

namespace Peernet.Browser.Application.Models
{
    public class ApiProfileData
    {
        public List<Field> Fields { get; set; }

        public List<Blob> Blobs { get; set; }

        public BlockchainStatus Status { get; set; }
    }
}
