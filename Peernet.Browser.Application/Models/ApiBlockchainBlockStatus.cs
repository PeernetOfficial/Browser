using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.Models
{
    public class ApiBlockchainBlockStatus
    {
        public BlockchainStatus Status { get; set; }

        public int Height { get; set; }
    }
}
