using Peernet.Browser.Application.Managers;
using Peernet.Browser.Infrastructure.Http;
using Peernet.Browser.Models.Domain.Common;
using Peernet.Browser.Models.Domain.Warehouse;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Peernet.Browser.Infrastructure.Clients
{
    internal class WarehouseClient : ClientBase, IWarehouseClient
    {
        private readonly IHttpExecutor httpExecutor;

        public WarehouseClient(ISettingsManager settingsManager)
        {
            httpExecutor = new HttpExecutor(settingsManager);
        }

        public override string CoreSegment => "warehouse";

        public async Task<WarehouseResult> Create(ApiBlockRecordFile file)
        {
            var content = JsonContent.Create(file);

            return await httpExecutor.GetResult<WarehouseResult>(HttpMethod.Post, GetRelativeRequestPath("create"),
                content: content);
        }
    }
}