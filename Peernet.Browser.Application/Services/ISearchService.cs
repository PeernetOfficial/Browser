using Peernet.Browser.Application.Models;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.Services
{
    public interface ISearchService
    {
        void Terminate(string id);

        SearchResultModel Search(SearchFilterResultModel model);

        Task<SearchResultModel> SearchAsync(SearchFilterResultModel model);
    }
}