using System.Threading.Tasks;
using Peernet.Browser.Application.Models;

namespace Peernet.Browser.Application.Services
{
    public interface IExploreService
    {
        Task<SearchResult> GetFiles(int limit, int? type = null);
    }
}
