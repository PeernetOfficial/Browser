using Peernet.Browser.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peernet.Browser.Infrastructure
{
    public class SocketClientConfigProvider : ISocketClientConfigProvider
    {
        public SocketClientConfigProvider(string uri)
        {
            Uri = uri;
        }

        public string Uri { get; private set; }
    }
}
