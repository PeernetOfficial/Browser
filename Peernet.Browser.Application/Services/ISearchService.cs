using System.Threading.Tasks;
using Peernet.Browser.Models.Presentation.Home;

namespace Peernet.Browser.Application.Services
{
    public interface ISearchService
    {
        Task Terminate(string id);

        Task<SearchResultModel> Search(SearchFilterResultModel model);

        Task<SearchResultModel> SearchAsync(SearchFilterResultModel model);
    }
}