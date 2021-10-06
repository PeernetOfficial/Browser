using System.Threading.Tasks;
using Peernet.Browser.Models;
using Peernet.Browser.Models.Domain;

namespace Peernet.Browser.Application.Facades
{
    public interface IExploreFacade
    {
        Task<SearchResult> GetFiles(int limit, int? type = null);
    }
}