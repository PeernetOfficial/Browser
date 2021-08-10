using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.Models
{
    public class Status
    {
        public int StatusCode { get; set; }
        public bool IsConnected { get; set; }
        public ulong CountPeerList { get; set; }
    }
}
