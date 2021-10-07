using Peernet.Browser.Application.Managers;
using Peernet.Browser.Infrastructure.Http;
using Peernet.Browser.Models.Domain;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Peernet.Browser.Infrastructure.Wrappers
{
    internal class ExploreWrapper : WrapperBase, IExploreWrapper
    {
        private readonly IHttpExecutor httpExecutor;

        public ExploreWrapper(ISettingsManager settingsManager)
        {
            httpExecutor = new HttpExecutor(settingsManager);
        }

        public override string CoreSegment => "explore";

        public async Task<SearchResult> GetFiles(int limit, int? type = null)
        {
            var parameters = new Dictionary<string, string>
            {
                [nameof(limit)] = limit.ToString()
            };

            if (type != null)
            {
                parameters.Add("type", type.ToString());
            }

            return await httpExecutor.GetResult<SearchResult>(HttpMethod.Get, GetRelativeRequestPath(string.Empty), parameters);
        }
    }
}