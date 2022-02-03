using Peernet.Browser.Models.Domain.File;
using System.Threading.Tasks;

namespace Peernet.Browser.Infrastructure.Clients
{
    internal interface IFileClient
    {
        Task<ApiResponseFileFormat> GetFormat(string path);
    }
}