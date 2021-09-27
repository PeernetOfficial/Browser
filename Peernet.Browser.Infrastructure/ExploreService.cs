using System.Threading.Tasks;
using Peernet.Browser.Application.Extensions;
using Peernet.Browser.Application.Http;
using Peernet.Browser.Application.Models;
using Peernet.Browser.Application.Services;
using RestSharp;

namespace Peernet.Browser.Infrastructure
{
    public class ExploreService : ServiceBase, IExploreService
    {
        public ExploreService(IRestClientFactory restClientFactory, ICmdClient cmdClient)
            : base(restClientFactory, cmdClient)
        {
        }

        public SearchResult GetFiles(int limit, int? type = null)
        {
            var request = new RestRequest(CoreSegment, Method.GET);
            request.AddQueryParameter("limit", limit.ToString());

            if (type != null)
            {
                request.AddQueryParameter("type", type.ToString());
            }
            
            return Task.Run(() => RestClient.GetAsync<SearchResult>(request)).GetResultBlockingWithoutContextSynchronization();
        }

        public override string CoreSegment => "explore";
    }
}
