using Peernet.Browser.Models.Domain;
using System.Threading.Tasks;

namespace Peernet.Browser.Infrastructure.Wrappers
{
    public interface IExploreWrapper
    {
        Task<SearchResult> GetFiles(int limit, int? type = null);
    }
}