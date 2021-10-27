using Peernet.Browser.Application.Managers;
using Peernet.Browser.Infrastructure.Http;
using Peernet.Browser.Models.Domain.Blockchain;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Peernet.Browser.Infrastructure.Clients
{
    internal class AccountClient : ClientBase, IAccountClient
    {
        private readonly IHttpExecutor httpExecutor;

        public AccountClient(ISettingsManager settingsManager)
        {
            httpExecutor = new HttpExecutor(settingsManager);
        }

        public override string CoreSegment => "account";

        public async Task Delete(bool confirm)
        {
            var parameters = new Dictionary<string, string>
            {
                [nameof(confirm)] = confirm ? "1" : "0"
            };

            await httpExecutor.GetResult<ApiBlockchainBlockStatus>(HttpMethod.Get, GetRelativeRequestPath("delete"), parameters);
        }
    }
}