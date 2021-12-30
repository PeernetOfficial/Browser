using Microsoft.Extensions.Logging;
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

        public AccountClient(IHttpExecutor httpExecutor)
        {
            this.httpExecutor = httpExecutor;
        }

        public override string CoreSegment => "account";

        public async Task Delete(bool confirm)
        {
            var parameters = new Dictionary<string, string>
            {
                [nameof(confirm)] = confirm ? "1" : "0"
            };

            await httpExecutor.GetResultAsync<ApiBlockchainBlockStatus>(HttpMethod.Get, GetRelativeRequestPath("delete"), parameters);
        }
    }
}