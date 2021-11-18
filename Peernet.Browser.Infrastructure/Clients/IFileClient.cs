using System.Threading.Tasks;
using Peernet.Browser.Models.Domain.File;

namespace Peernet.Browser.Infrastructure.Clients
{
    public interface IFileClient
    {
        Task<ApiResponseFileFormat> GetFormat(string path);
    }
}