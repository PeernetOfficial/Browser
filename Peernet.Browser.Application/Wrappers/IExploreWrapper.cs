using Peernet.Browser.Models.Domain;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.Wrappers
{
    public interface IExploreWrapper
    {
        Task<SearchResult> GetFiles(int limit, int? type = null);
    }
}