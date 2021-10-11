using Peernet.Browser.Models.Presentation.Home;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.Services
{
    public interface ISearchService
    {
        Task<SearchResultModel> Search(SearchFilterResultModel model);
    }
}