using System;
using System.Collections.Generic;
using Peernet.Browser.Application.Managers;
using Peernet.Browser.Infrastructure.Http;
using Peernet.Browser.Models.Domain.Warehouse;
using System.Net.Http;
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

        public async Task<WarehouseResult> Create(byte[] fileContent)
        {
            var content = new ByteArrayContent(fileContent);

            return await httpExecutor.GetResultAsync<WarehouseResult>(HttpMethod.Post, GetRelativeRequestPath("create"),
                content: content);
        }

        public async Task<WarehouseResult> ReadPath(byte[] hash, string path)
        {
            var parameters = new Dictionary<string, string>
            {
                [nameof(hash)] = Convert.ToHexString(hash),
                [nameof(path)] = path
            };

            return await httpExecutor.GetResultAsync<WarehouseResult>(HttpMethod.Get, GetRelativeRequestPath("read/path"),
                parameters);
        }
    }
}