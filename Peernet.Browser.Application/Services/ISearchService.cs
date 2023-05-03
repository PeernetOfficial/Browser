using Peernet.SDK.Models.Presentation.Home;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.Services
{
    public interface ISearchService
    {
        Task<SearchResultModel> Search(SearchFilterResultModel model);

        Task Terminate(string id);

        IDictionary<FilterType, int> GetEmptyStats();

        Task<string> CreateSnapshot(SearchResultModel searchResultModel);
    }
}