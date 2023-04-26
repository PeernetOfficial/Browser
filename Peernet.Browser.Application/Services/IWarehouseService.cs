using Peernet.SDK.Models.Domain.Common;
using Peernet.SDK.Models.Domain.Warehouse;
using Peernet.SDK.Models.Presentation.Footer;
using System;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.Services
{
    public interface IWarehouseService
    {
        Task<WarehouseResult> Create(Guid id, FileModel file);

        Task<WarehouseResult> ReadPath(ApiFile file);
    }
}