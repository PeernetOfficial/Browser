using Peernet.Browser.Models.Presentation.Home;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.Services
{
    public interface ISearchService
    {
        Task<SearchResultModel> Search(SearchFilterResultModel model);

        void Terminate(string id);

        IDictionary<FiltersType, int> GetEmptyStats();
    }
}