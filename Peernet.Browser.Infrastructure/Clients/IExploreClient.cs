using Peernet.Browser.Models.Domain.Search;
using System.Threading.Tasks;

namespace Peernet.Browser.Infrastructure.Clients
{
    public interface IExploreClient
    {
        Task<SearchResult> GetFiles(int limit, int? type = null);
    }
}