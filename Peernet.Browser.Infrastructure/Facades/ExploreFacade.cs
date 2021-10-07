using Peernet.Browser.Application.Facades;
using Peernet.Browser.Models.Domain;
using System.Threading.Tasks;
using Peernet.Browser.Application.Managers;
using Peernet.Browser.Application.Wrappers;
using Peernet.Browser.Infrastructure.Wrappers;

namespace Peernet.Browser.Infrastructure.Facades
{
    public class ExploreFacade : IExploreFacade
    {
        private readonly IExploreWrapper exploreWrapper;

        public ExploreFacade(ISettingsManager settingsManager)
        {
            this.exploreWrapper = new ExploreWrapper(settingsManager);
        }

        // todo: it should return UI model
        public async Task<SearchResult> GetFiles(int limit, int? type = null)
        {
            return await exploreWrapper.GetFiles(limit, type);
        }
    }
}