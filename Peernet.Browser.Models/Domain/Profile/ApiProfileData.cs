using Peernet.Browser.Models.Domain.Blockchain;
using System.Collections.Generic;

namespace Peernet.Browser.Models.Domain.Profile
{
    public class ApiProfileData
    {
        public List<ApiBlockRecordProfile> Fields { get; set; }

        public BlockchainStatus Status { get; set; }
    }
}