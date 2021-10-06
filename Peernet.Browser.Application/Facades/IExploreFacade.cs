using Peernet.Browser.Models.Domain;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.Facades
{
    public interface IExploreFacade
    {
        Task<SearchResult> GetFiles(int limit, int? type = null);
    }
}