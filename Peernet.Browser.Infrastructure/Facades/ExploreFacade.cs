using Peernet.Browser.Application.Facades;
using Peernet.Browser.Application.Wrappers;
using Peernet.Browser.Models.Domain;
using System.Threading.Tasks;

namespace Peernet.Browser.Infrastructure.Facades
{
    public class ExploreFacade : IExploreFacade
    {
        private readonly IExploreWrapper exploreWrapper;

        public ExploreFacade(IExploreWrapper exploreWrapper)
        {
            this.exploreWrapper = exploreWrapper;
        }

        // todo: it should return UI model
        public async Task<SearchResult> GetFiles(int limit, int? type = null)
        {
            return await exploreWrapper.GetFiles(limit, type);
        }
    }
}