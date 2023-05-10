using Peernet.SDK.Models.Presentation.Footer;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.Interfaces
{
    public interface IShareableContent
    {
        Task<FileModel> CreateResultsSnapshot();
    }
}
