using System.Threading.Tasks;
using Peernet.Browser.Models.Domain.Search;

namespace Peernet.Browser.Infrastructure.Clients
{
    public interface IExploreClient
    {
        Task<SearchResult> GetFiles(int limit, int? type = null);
    }
}