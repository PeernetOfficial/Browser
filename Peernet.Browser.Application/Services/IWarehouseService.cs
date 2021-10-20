using Peernet.Browser.Models.Domain.Common;
using Peernet.Browser.Models.Domain.Warehouse;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.Services
{
    public interface IWarehouseService
    {
        Task<WarehouseResult> Create(ApiBlockRecordFile file);
    }
}