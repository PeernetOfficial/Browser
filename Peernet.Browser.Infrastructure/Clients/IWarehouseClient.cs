using Peernet.Browser.Models.Domain.Warehouse;
using System.IO;
using System.Threading.Tasks;

namespace Peernet.Browser.Infrastructure.Clients
{
    internal interface IWarehouseClient
    {
        Task<WarehouseResult> Create(Stream stream);

        Task<WarehouseResult> ReadPath(byte[] hash, string path);
    }
}