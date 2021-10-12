using System.Collections.Generic;
using System.Threading.Tasks;
using Peernet.Browser.Models.Presentation.Footer;

namespace Peernet.Browser.Application.Services
{
    public interface IExploreService
    {
        Task<List<DownloadModel>> GetFiles(int limit, int? type = null);
    }
}