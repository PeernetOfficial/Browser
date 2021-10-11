using System.Collections.Generic;
using Peernet.Browser.Models.Domain.Blockchain;

namespace Peernet.Browser.Models.Domain.Profile
{
    public class ApiProfileData
    {
        public List<ApiBlockRecordProfile> Fields { get; set; }

        public BlockchainStatus Status { get; set; }
    }
}