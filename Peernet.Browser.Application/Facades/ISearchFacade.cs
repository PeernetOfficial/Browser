using System.Threading.Tasks;
using Peernet.Browser.Models.Domain;
using Peernet.Browser.Models.Presentation;
using Peernet.Browser.Models.Presentation.Home;

namespace Peernet.Browser.Application.Facades
{
    public interface ISearchFacade
    {
        Task Terminate(string id);

        Task<SearchResultModel> Search(SearchFilterResultModel model);
    }
}