using Peernet.Browser.Application.Models;

namespace Peernet.Browser.Application.Services
{
    public interface ISearchService
    {
        void Terminate(string id);

        SearchResultModel Search(SearchFilterResultModel model);
    }
}