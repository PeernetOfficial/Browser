using System.Threading.Tasks;
using Peernet.Browser.Models.Domain.Search;

namespace Peernet.Browser.Application.Services
{
    public interface IExploreService
    {
        Task<SearchResult> GetFiles(int limit, int? type = null);
    }
}