using Peernet.Browser.Application.Managers;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Infrastructure.Clients;
using Peernet.Browser.Models.Domain.Search;
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

        // todo: it should return UI model
        public async Task<SearchResult> GetFiles(int limit, int? type = null)
        {
            return await exploreClient.GetFiles(limit, type);
        }
    }
}