using Peernet.Browser.Application.Managers;
using Peernet.Browser.Infrastructure.Http;
using Peernet.Browser.Models.Domain.Shutdown;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Peernet.Browser.Infrastructure.Clients
{
    internal class ShutdownClient : ClientBase, IShutdownClient
    {
        private readonly HttpExecutor httpExecutor;

        public override string CoreSegment => "shutdown";

        public ShutdownClient(ISettingsManager settingsManager)
        {
            httpExecutor = new HttpExecutor(settingsManager);
        }

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