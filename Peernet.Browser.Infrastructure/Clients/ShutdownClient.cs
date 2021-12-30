using Peernet.Browser.Infrastructure.Http;
using Peernet.Browser.Models.Domain.Shutdown;
using System.Collections.Generic;
using System.Net.Http;

namespace Peernet.Browser.Infrastructure.Clients
{
    internal class ShutdownClient : ClientBase, IShutdownClient
    {
        private readonly IHttpExecutor httpExecutor;

        public ShutdownClient(IHttpExecutor httpExecutor)
        {
            this.httpExecutor = httpExecutor;
        }

        public override string CoreSegment => "shutdown";
        public ApiShutdownStatus GetAction(ShutdownAction action)
        {
            var parameters = new Dictionary<string, string>
            {
                [nameof(action)] = ((int)action).ToString()
            };

            return httpExecutor.GetResult<ApiShutdownStatus>(HttpMethod.Get, GetRelativeRequestPath(string.Empty),
                parameters);
        }
    }
}