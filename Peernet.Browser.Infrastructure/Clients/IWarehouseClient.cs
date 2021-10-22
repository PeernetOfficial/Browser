using Peernet.Browser.Models.Domain.Common;
using Peernet.Browser.Models.Domain.Warehouse;
using System.Threading.Tasks;

namespace Peernet.Browser.Infrastructure.Clients
{
    internal interface IWarehouseClient
    {
        Task<WarehouseResult> Create(byte[] fileContent);
    }
}