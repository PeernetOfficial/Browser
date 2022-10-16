using Peernet.Browser.Application.Services;
using Peernet.SDK.Client.Clients;
using Peernet.SDK.Models.Domain.Common;
using Peernet.SDK.Models.Domain.Status;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Peernet.Browser.Infrastructure.Services
{
    internal class StatusService : IStatusService
    {
        private readonly IStatusClient statusClient;

        public StatusService(IStatusClient statusClient)
        {
            this.statusClient = statusClient;
        }

        public async Task<List<PeerStatus>> GetPeersStatus() => await statusClient.GetPeers();

        public async Task<ApiResponseStatus> GetStatus() => await statusClient.Get();
    }
}