using System.Collections.Generic;

namespace Peernet.Browser.Application.Models
{
    public class ApiProfileData
    {
        public List<ApiBlockRecordProfile> Fields { get; set; }

        public BlockchainStatus Status { get; set; }
    }
}
