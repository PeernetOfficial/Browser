using Peernet.Browser.Models.Domain.Warehouse;
using Peernet.Browser.Models.Presentation.Footer;
using System.Threading.Tasks;
using Peernet.Browser.Models.Domain.Common;

namespace Peernet.Browser.Application.Services
{
    public interface IWarehouseService
    {
        Task<WarehouseResult> Create(FileModel file);

        Task<WarehouseResult> ReadPath(ApiFile file);
    }
}