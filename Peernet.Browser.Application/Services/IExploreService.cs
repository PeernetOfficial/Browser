using Peernet.Browser.Application.Models;

namespace Peernet.Browser.Application.Services
{
    public interface IExploreService
    {
        SearchResult GetFiles(int limit, LowLevelFileType? type = null);
    }
}
