using Peernet.SDK.Models.Presentation.Footer;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.Services
{
    public interface IExploreService
    {
        Task<List<DownloadModel>> GetFiles(int limit, int? type = null);

        Task<List<DownloadModel>> GetPagedFiles(int offset, int limit, int? type = null);
    }
}