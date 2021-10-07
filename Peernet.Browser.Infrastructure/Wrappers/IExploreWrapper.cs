using System.Threading.Tasks;
using Peernet.Browser.Models.Domain;

namespace Peernet.Browser.Infrastructure.Wrappers
{
    public interface IExploreWrapper
    {
        Task<SearchResult> GetFiles(int limit, int? type = null);
    }
}