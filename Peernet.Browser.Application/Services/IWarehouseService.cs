using Peernet.Browser.Models.Domain.Warehouse;
using Peernet.Browser.Models.Presentation.Footer;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.Services
{
    public interface IWarehouseService
    {
        Task<WarehouseResult> Create(SharedNewFileModel file);
    }
}