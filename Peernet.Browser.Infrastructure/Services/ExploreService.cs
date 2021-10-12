using Peernet.Browser.Application.Managers;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Infrastructure.Clients;
using Peernet.Browser.Models.Presentation.Footer;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Peernet.Browser.Infrastructure.Services
{
    public class ExploreService : IExploreService
    {
        private readonly IExploreClient exploreClient;

        public ExploreService(ISettingsManager settingsManager)
        {
            exploreClient = new ExploreClient(settingsManager);
        }

        public async Task<List<DownloadModel>> GetFiles(int limit, int? type = null)
        {
            var files = (await exploreClient.GetFiles(limit, type)).Files;

            return files.Select(f => new DownloadModel(f)).ToList();
        }
    }
}