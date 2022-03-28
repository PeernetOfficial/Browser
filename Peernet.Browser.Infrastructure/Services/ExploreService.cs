using Peernet.Browser.Application.Services;
using Peernet.SDK.Client.Clients;
using Peernet.SDK.Models.Domain.Common;
using Peernet.SDK.Models.Presentation.Footer;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Peernet.Browser.Infrastructure.Services
{
    internal class ExploreService : IExploreService
    {
        private readonly IExploreClient exploreClient;

        public ExploreService(IExploreClient exploreClient)
        {
            this.exploreClient = exploreClient;
        }

        public async Task<List<DownloadModel>> GetFiles(int limit, int? type = null)
        {
            var files = (await exploreClient.GetFiles(limit, type))?.Files ?? Enumerable.Empty<ApiFile>();

            return files.Select(f => new DownloadModel(f)).ToList();
        }
    }
}