using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.Services
{
    public interface ISettingsManager
    {
        string ApiUrl { get;set; }
        string SocketUrl { get; set; }
    }
}
