using System.Threading.Tasks;
using Peernet.Browser.Models;
using Peernet.Browser.Models.Domain;

namespace Peernet.Browser.Application.Wrappers
{
    public interface IExploreWrapper
    {
        Task<SearchResult> GetFiles(int limit, int? type = null);
    }
}
